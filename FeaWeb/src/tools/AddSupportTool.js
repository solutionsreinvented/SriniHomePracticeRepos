import { appState } from '../core/AppState.js';
import { eventBus } from '../core/EventBus.js';
import * as THREE   from 'three';

export class AddSupportTool {
  constructor(viewport) { this.vp = viewport; this._type = 'pinned'; }

  activate(opts = {}) {
    this._type = opts.type || 'pinned';
    this.vp.canvas.style.cursor = 'crosshair';
    eventBus.emit('status:hint', `Click a node to add ${this._type} support. Esc to cancel.`);
  }

  deactivate() { this.vp.canvas.style.cursor = 'default'; }
  onMouseMove() {}

  onClick(e, hit) {
    // Try to pick an existing node
    if (hit && hit.object.userData.type === 'node') {
      appState.addSupport(hit.object.userData.id, this._type);
      return;
    }
    // Or snap to nearest grid and create node+support
    const snap = this.vp.snapping.snap(e);
    if (!snap) return;
    let nodeId = snap.nodeId;
    if (!nodeId) {
      const n = appState.addNode(snap.x, snap.y, snap.z);
      nodeId = n.id;
    }
    appState.addSupport(nodeId, this._type);
  }
}

export class AddLoadTool {
  constructor(viewport) { this.vp = viewport; }

  activate() {
    this.vp.canvas.style.cursor = 'crosshair';
    eventBus.emit('status:hint', 'Click a node to add a downward point load (10 kN). Esc to cancel.');
  }
  deactivate() { this.vp.canvas.style.cursor = 'default'; }
  onMouseMove() {}

  onClick(e, hit) {
    let nodeId = null;
    if (hit && hit.object.userData.type === 'node') {
      nodeId = hit.object.userData.id;
    } else {
      const snap = this.vp.snapping.snap(e);
      if (!snap) return;
      nodeId = snap.nodeId;
      if (!nodeId) {
        const n = appState.addNode(snap.x, snap.y, snap.z);
        nodeId = n.id;
      }
    }
    // Default: 10 kN downward (Y-direction, negative)
    appState.addNodeLoad(nodeId, -10000, 0, 0); // Fy = -10kN
  }
}
