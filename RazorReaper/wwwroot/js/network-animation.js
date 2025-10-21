class NetworkAnimation {
    constructor() {
        this.canvas = document.getElementById('network-canvas');
        this.ctx = this.canvas.getContext('2d');
        this.mouseX = 0;
        this.mouseY = 0;
        this.nodes = [];
        this.trails = [];
        this.animationPaused = false;
        this.animationId = null;
        this.time = 0;

        this.init();
    }

    init() {
        this.setupEventListeners();
        this.resizeCanvas();
        this.createNodes();
        this.animate();
    }

    setupEventListeners() {
        document.addEventListener('mousemove', (e) => {
            this.mouseX = e.clientX;
            this.mouseY = e.clientY;
        });

        document.addEventListener('mouseenter', (e) => {
            this.mouseX = e.clientX;
            this.mouseY = e.clientY;
        });

        window.addEventListener('resize', () => {
            this.resizeCanvas();
            this.createNodes();
        });
    }

    resizeCanvas() {
        this.canvas.width = window.innerWidth;
        this.canvas.height = window.innerHeight;
    }

    createNodes() {
        this.nodes = [];
        const nodeCount = Math.floor((this.canvas.width * this.canvas.height) / 20000);

        for (let i = 0; i < nodeCount; i++) {
            this.nodes.push({
                x: Math.random() * this.canvas.width,
                y: Math.random() * this.canvas.height,
                vx: (Math.random() - 0.5) * 0.6,
                vy: (Math.random() - 0.5) * 0.6,
                size: Math.random() * 0.8 + 0.4,
                pulseOffset: Math.random() * Math.PI * 2,
                trail: []
            });
        }
    }

    createRipple(x, y) {

    }

    createParticle(x1, y1, x2, y2) {

    }

    drawElectricArc(x1, y1, x2, y2, intensity) {
        const segments = 8;
        const displacement = 3 * intensity;

        let points = [{ x: x1, y: y1 }];

        for (let i = 1; i < segments; i++) {
            const t = i / segments;
            const midX = x1 + (x2 - x1) * t;
            const midY = y1 + (y2 - y1) * t;

            const offsetX = (Math.random() - 0.5) * displacement;
            const offsetY = (Math.random() - 0.5) * displacement;

            points.push({ x: midX + offsetX, y: midY + offsetY });
        }

        points.push({ x: x2, y: y2 });

        this.ctx.beginPath();
        this.ctx.moveTo(points[0].x, points[0].y);

        for (let i = 1; i < points.length; i++) {
            this.ctx.lineTo(points[i].x, points[i].y);
        }

        this.ctx.stroke();
    }

    animate() {
        this.time += 0.02;

        this.ctx.fillStyle = '#0a0a0a';
        this.ctx.fillRect(0, 0, this.canvas.width, this.canvas.height);

        this.nodes.forEach((node, i) => {
            const dx = this.mouseX - node.x;
            const dy = this.mouseY - node.y;
            const distance = Math.sqrt(dx * dx + dy * dy);

            if (distance < 150) {
                const orbitForce = 0.0006;
                const attractForce = (150 - distance) / 150 * 0.012;

                const angle = Math.atan2(dy, dx);
                const perpAngle = angle + Math.PI / 2;

                node.vx += Math.cos(angle) * attractForce + Math.cos(perpAngle) * orbitForce;
                node.vy += Math.sin(angle) * attractForce + Math.sin(perpAngle) * orbitForce;
            }

            node.x += node.vx;
            node.y += node.vy;

            if (node.x < 0 || node.x > this.canvas.width) node.vx *= -1;
            if (node.y < 0 || node.y > this.canvas.height) node.vy *= -1;

            node.trail.push({ x: node.x, y: node.y, life: 1.0 });
            if (node.trail.length > 8) node.trail.shift();

            node.trail.forEach((point, index) => {
                point.life -= 0.15;
                if (point.life > 0) {
                    const trailSize = node.size * 0.3 * point.life;
                    this.ctx.beginPath();
                    this.ctx.arc(point.x, point.y, trailSize, 0, Math.PI * 2);
                    this.ctx.fillStyle = `rgba(255, 255, 255, ${point.life * 0.3})`;
                    this.ctx.fill();
                }
            });

            const pulseSize = node.size + Math.sin(this.time * 3 + node.pulseOffset) * 0.3;
            this.ctx.beginPath();
            this.ctx.arc(node.x, node.y, pulseSize, 0, Math.PI * 2);
            this.ctx.fillStyle = distance < 80 ? 'rgba(255, 255, 255, 0.9)' : 'rgba(255, 255, 255, 0.6)';
            this.ctx.fill();

            this.nodes.slice(i + 1).forEach(otherNode => {
                const nodeDx = otherNode.x - node.x;
                const nodeDy = otherNode.y - node.y;
                const nodeDistance = Math.sqrt(nodeDx * nodeDx + nodeDy * nodeDy);

                const mouseDistToNode = Math.sqrt((this.mouseX - node.x) ** 2 + (this.mouseY - node.y) ** 2);
                const mouseDistToOther = Math.sqrt((this.mouseX - otherNode.x) ** 2 + (this.mouseY - otherNode.y) ** 2);

                let connectionDistance = 130;
                let useElectric = false;

                if (mouseDistToNode < 120 && mouseDistToOther < 120) {
                    connectionDistance = 180;
                    useElectric = Math.random() < 0.3;
                }

                if (nodeDistance < connectionDistance) {
                    const alpha = (1 - nodeDistance / connectionDistance) * 0.4;
                    this.ctx.strokeStyle = `rgba(255, 255, 255, ${alpha})`;
                    this.ctx.lineWidth = 0.5;

                    if (useElectric) {
                        this.drawElectricArc(node.x, node.y, otherNode.x, otherNode.y, alpha);
                    } else {
                        this.ctx.beginPath();
                        this.ctx.moveTo(node.x, node.y);
                        this.ctx.lineTo(otherNode.x, otherNode.y);
                        this.ctx.stroke();
                    }
                }
            });
        });

        this.animationId = requestAnimationFrame(() => this.animate());
    }

    pause() {
        if (this.animationId) {
            cancelAnimationFrame(this.animationId);
        }
    }

    resume() {
        this.animate();
    }

    destroy() {
        this.pause();
    }
}

document.addEventListener('DOMContentLoaded', () => {
    setTimeout(() => {
        if (document.getElementById('network-canvas')) {
            window.networkAnimation = new NetworkAnimation();
        }
    }, 100);
});