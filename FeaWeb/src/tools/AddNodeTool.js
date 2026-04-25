import * as THREE   from 'three';
import { appState }  from '../core/AppState.js';
import { eventBus }  from '../core/EventBus.js';

export class AddNodeTool {
  constructor(viewport) {
    this.vp = viewport;
    this._cursor = null;
  }

  activate() {
    this.vp.canvas.style.cursor = 'crosshair';
    this._cursor = this.vp.snapping.getCursorMesh();
    this.vp.addTempObject(this._cursor);
    eventBus.emit('status:hint', 'Click in viewport to place node. Press Esc to cancel.');
  }

  deactivate() {
    this.vp.canvas.style.cursor = 'default';
    this.vp.clearTempObjects();
    this._cursor = null;
  }

  onClick(e, hit) {
    const snap = this.vp.snapping.snap(e);
    if (!snap) return;
    const n = appState.addNode(snap.x, snap.y, snap.z);
    // Brief flash animation on new node
    const mesh = this.vp.nodeRenderer._meshes.get(n.id);
    if (mesh) {
      const mat = mesh.material;
      mat.emissiveIntensity = 1.0;
      setTimeout(() => { mat.emissiveIntensity = 0.3; this.vp.markDirty(); }, 200);
    }
    eventBus.emit('cursor:moved', snap);
  }

  onMouseMove(e, worldPos) {
    const snap = this.vp.snapping.snap(e);
    if (!snap || !this._cursor) return;
    this._cursor.position.set(snap.x, snap.y, snap.z);
    this.vp.markDirty();
    eventBus.emit('cursor:moved', snap);
  }
}
