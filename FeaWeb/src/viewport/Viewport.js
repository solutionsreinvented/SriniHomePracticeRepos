import * as THREE from 'three';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';
import { eventBus } from '../core/EventBus.js';
import { NodeRenderer }    from './NodeRenderer.js';
import { ElementRenderer } from './ElementRenderer.js';
import { SupportRenderer } from './SupportRenderer.js';
import { LoadRenderer }    from './LoadRenderer.js';
import { ResultRenderer }  from './ResultRenderer.js';
import { SelectionManager } from './SelectionManager.js';
import { SnappingEngine }  from './SnappingEngine.js';
import { appState }        from '../core/AppState.js';

export class Viewport {
  constructor(canvas) {
    this.canvas  = canvas;
    this._dirty  = true;
    this._raf    = null;
    this._init();
    this._bindEvents();
  }

  _init() {
    /* Renderer */
    this.renderer = new THREE.WebGLRenderer({
      canvas: this.canvas,
      antialias: true,
      alpha: false,
      powerPreference: 'high-performance'
    });
    this.renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
    this.renderer.shadowMap.enabled = true;
    this.renderer.shadowMap.type = THREE.PCFSoftShadowMap;
    this.renderer.setClearColor(0x161920, 1);

    /* Scene */
    this.scene = new THREE.Scene();
    this.scene.fog = new THREE.FogExp2(0x161920, 0.008);

    /* Camera — Y-up */
    const w = this.canvas.clientWidth || 800;
    const h = this.canvas.clientHeight || 600;
    this.camera = new THREE.PerspectiveCamera(45, w / h, 0.01, 5000);
    this.camera.position.set(8, 6, 10);
    this.camera.lookAt(0, 0, 0);

    /* Orbit Controls */
    this.controls = new OrbitControls(this.camera, this.renderer.domElement);
    this.controls.enableDamping = true;
    this.controls.dampingFactor = 0.08;
    this.controls.screenSpacePanning = true;
    this.controls.minDistance = 0.1;
    this.controls.maxDistance = 2000;
    this.controls.addEventListener('change', () => { this._dirty = true; });

    /* Lights */
    this._addLights();

    /* Grid */
    this._addGrid();

    /* Axis indicator */
    this._addAxisIndicator();

    /* Entity groups */
    this.groups = {
      nodes:    new THREE.Group(),
      elements: new THREE.Group(),
      supports: new THREE.Group(),
      loads:    new THREE.Group(),
      results:  new THREE.Group(),
      labels:   new THREE.Group(),
      temp:     new THREE.Group(), // preview/ghost geometry
    };
    Object.values(this.groups).forEach(g => this.scene.add(g));

    /* Sub-systems */
    this.nodeRenderer    = new NodeRenderer(this.groups.nodes, this.scene);
    this.elementRenderer = new ElementRenderer(this.groups.elements, this.scene);
    this.supportRenderer = new SupportRenderer(this.groups.supports, this.scene);
    this.loadRenderer    = new LoadRenderer(this.groups.loads, this.scene);
    this.resultRenderer  = new ResultRenderer(this.groups.results, this.scene);
    this.selectionMgr    = new SelectionManager(this);
    this.snapping        = new SnappingEngine(this);

    /* Resize */
    this._onResize();
    window.addEventListener('resize', () => this._onResize());

    /* Render loop */
    this._loop();
  }

  _addLights() {
    const ambient = new THREE.AmbientLight(0xffffff, 0.6);
    this.scene.add(ambient);

    const sun = new THREE.DirectionalLight(0xffffff, 1.2);
    sun.position.set(20, 40, 20);
    sun.castShadow = true;
    sun.shadow.mapSize.set(2048, 2048);
    sun.shadow.camera.far = 500;
    this.scene.add(sun);

    const fill = new THREE.DirectionalLight(0x8899cc, 0.4);
    fill.position.set(-15, 10, -10);
    this.scene.add(fill);
  }

  _addGrid() {
    /* Major grid */
    const major = new THREE.GridHelper(100, 20, 0x2a3040, 0x222840);
    major.position.y = 0;
    this.scene.add(major);
    this._grid = major;

    /* Minor grid */
    const minor = new THREE.GridHelper(100, 100, 0x1e2436, 0x1e2436);
    minor.position.y = 0.001;
    this.scene.add(minor);

    /* Ground plane (invisible, used for raycasting) */
    this.groundPlane = new THREE.Mesh(
      new THREE.PlaneGeometry(5000, 5000),
      new THREE.MeshBasicMaterial({ visible: false, side: THREE.DoubleSide })
    );
    this.groundPlane.rotation.x = -Math.PI / 2;
    this.groundPlane.name = '__ground__';
    this.scene.add(this.groundPlane);
  }

  _addAxisIndicator() {
    /* Small axes in bottom-left corner via a separate scene + camera */
    this.axisScene  = new THREE.Scene();
    this.axisCamera = new THREE.PerspectiveCamera(50, 1, 0.01, 100);
    this.axisCamera.position.set(0, 0, 2.5);

    const axes = new THREE.AxesHelper(1);
    /* Override colors: X=red, Y=green, Z=blue */
    const colors = axes.geometry.attributes.color;
    // Already set by AxesHelper
    this.axisScene.add(axes);

    // Labels for axes
    const makeLabel = (text, pos, color) => {
      const canvas = document.createElement('canvas');
      canvas.width = canvas.height = 64;
      const ctx = canvas.getContext('2d');
      ctx.fillStyle = color;
      ctx.font = 'bold 40px Inter, sans-serif';
      ctx.textAlign = 'center';
      ctx.textBaseline = 'middle';
      ctx.fillText(text, 32, 32);
      const tex = new THREE.CanvasTexture(canvas);
      const sp  = new THREE.Sprite(new THREE.SpriteMaterial({ map: tex, depthTest: false }));
      sp.position.copy(pos);
      sp.scale.set(.3, .3, .3);
      this.axisScene.add(sp);
    };
    makeLabel('X', new THREE.Vector3(1.4, 0, 0), '#f87171');
    makeLabel('Y', new THREE.Vector3(0, 1.4, 0), '#4ade80');
    makeLabel('Z', new THREE.Vector3(0, 0, 1.4), '#60a5fa');
  }

  _loop() {
    this._raf = requestAnimationFrame(() => this._loop());
    this.controls.update();
    if (!this._dirty) return;
    this._dirty = false;
    this._render();
  }

  _render() {
    const w = this.canvas.clientWidth;
    const h = this.canvas.clientHeight;
    this.renderer.setViewport(0, 0, w, h);
    this.renderer.setScissor(0, 0, w, h);
    this.renderer.setScissorTest(true);
    this.renderer.render(this.scene, this.camera);

    /* Axis indicator — bottom-left 100×100px */
    const ax = 100, ay = 100;
    this.renderer.setViewport(0, 0, ax, ay);
    this.renderer.setScissor(0, 0, ax, ay);
    this.axisCamera.quaternion.copy(this.camera.quaternion);
    this.renderer.clearDepth();
    this.renderer.render(this.axisScene, this.axisCamera);
  }

  _onResize() {
    const w = this.canvas.clientWidth;
    const h = this.canvas.clientHeight;
    this.renderer.setSize(w, h, false);
    this.camera.aspect = w / h;
    this.camera.updateProjectionMatrix();
    this._dirty = true;
  }

  markDirty() { this._dirty = true; }

  /* Public helpers */
  setViewDirection(dir) {
    const d = 15;
    const positions = {
      'iso':   [d, d, d],
      'front': [0, d/2, d],
      'back':  [0, d/2, -d],
      'top':   [0, d*2, 0.001],
      'left':  [-d, d/2, 0],
      'right': [d,  d/2, 0],
    };
    const p = positions[dir] || positions['iso'];
    this.camera.position.set(...p);
    this.controls.target.set(0, 0, 0);
    this.controls.update();
    this._dirty = true;
  }

  fitAll() {
    const box = new THREE.Box3();
    this.scene.traverse(o => {
      if (o.isMesh && o.name !== '__ground__') box.expandByObject(o);
    });
    if (box.isEmpty()) return;
    const sphere = box.getBoundingSphere(new THREE.Sphere());
    const r = Math.max(sphere.radius, 1);
    this.controls.target.copy(sphere.center);
    const dist = r / Math.sin((this.camera.fov * Math.PI / 180) / 2);
    const dir  = this.camera.position.clone().sub(sphere.center).normalize();
    this.camera.position.copy(sphere.center).addScaledVector(dir, dist);
    this.controls.update();
    this._dirty = true;
  }

  /* Full model refresh */
  refreshAll() {
    const m = appState.model;
    const s = appState.displaySettings;
    this.nodeRenderer.refresh(m);
    this.elementRenderer.refresh(m);
    this.supportRenderer.refresh(m, s);
    this.loadRenderer.refresh(m, s);
    this.markDirty();
  }

  addTempObject(obj)    { this.groups.temp.add(obj); this.markDirty(); }
  clearTempObjects()    { this.groups.temp.clear();   this.markDirty(); }

  _bindEvents() {
    eventBus.on('model:changed', () => this.refreshAll());
    eventBus.on('analysis:complete', () => {
      this.resultRenderer.refresh(appState.model, appState.analysisResults, appState.displaySettings);
      this.markDirty();
    });
  }

  dispose() {
    cancelAnimationFrame(this._raf);
    this.controls.dispose();
    this.renderer.dispose();
  }
}
