import * as THREE from 'three';
import { eventBus } from '../core/EventBus.js';
import { appState }  from '../core/AppState.js';

export class SelectionManager {
  constructor(viewport) {
    this.vp      = viewport;
    this._ray    = new THREE.Raycaster();
    this._ray.params.Line.threshold = 0.05;
    this._mouse  = new THREE.Vector2();
    this._bind();
  }

  _bind() {
    const canvas = this.vp.canvas;
    canvas.addEventListener('click', e => this._onClick(e));
    canvas.addEventListener('mousemove', e => this._onMove(e));
  }

  _toNDC(e) {
    const rect = this.vp.canvas.getBoundingClientRect();
    this._mouse.x =  ((e.clientX - rect.left)  / rect.width)  * 2 - 1;
    this._mouse.y = -((e.clientY - rect.top)   / rect.height) * 2 + 1;
  }

  _getPickables() {
    return [
      ...this.vp.nodeRenderer.getMeshes(),
      ...this.vp.elementRenderer.getMeshes(),
      ...this.vp.supportRenderer.getMeshes(),
    ];
  }

  _onClick(e) {
    // Let active tool handle it first
    const tool = appState.activeTool;
    if (tool && tool.onClick) { tool.onClick(e, this._hitTest(e)); return; }

    const hit = this._hitTest(e);
    if (hit) {
      const { type, id } = hit.object.userData;
      if (e.ctrlKey || e.metaKey) appState.addToSelection(type, id);
      else appState.selectSingle(type, id);
      this._updateVisual();
    } else {
      if (!e.ctrlKey && !e.metaKey) {
        appState.clearSelection();
        this._updateVisual();
      }
    }
  }

  _onMove(e) {
    this._toNDC(e);
    const tool = appState.activeTool;
    if (tool && tool.onMouseMove) tool.onMouseMove(e, this._getWorldPos(e));

    const hit = this._hitTest(e);
    if (hit) {
      this.vp.canvas.style.cursor = 'pointer';
    } else {
      this.vp.canvas.style.cursor = appState.activeTool ? 'crosshair' : 'default';
    }
  }

  _hitTest(e) {
    this._toNDC(e);
    this._ray.setFromCamera(this._mouse, this.vp.camera);
    const hits = this._ray.intersectObjects(this._getPickables(), true);
    return hits.length ? hits[0] : null;
  }

  _getWorldPos(e) {
    this._toNDC(e);
    this._ray.setFromCamera(this._mouse, this.vp.camera);
    const hits = this._ray.intersectObject(this.vp.groundPlane);
    return hits.length ? hits[0].point : null;
  }

  _updateVisual() {
    // Reset all
    const m = appState.model;
    for (const [id] of m.nodes)    this.vp.nodeRenderer.setSelected(id, false);
    for (const [id] of m.elements) this.vp.elementRenderer.setSelected(id, false);
    // Apply selection
    for (const sel of appState.selection) {
      if (sel.type === 'node')    this.vp.nodeRenderer.setSelected(sel.id, true);
      if (sel.type === 'element') this.vp.elementRenderer.setSelected(sel.id, true);
    }
    this.vp.markDirty();
  }
}
