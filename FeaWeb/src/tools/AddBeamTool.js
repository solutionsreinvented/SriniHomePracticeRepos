import * as THREE  from 'three';
import { appState } from '../core/AppState.js';
import { eventBus } from '../core/EventBus.js';

export class AddBeamTool {
  constructor(viewport) {
    this.vp      = viewport;
    this._phase  = 0;       // 0=pick start, 1=pick end
    this._startNodeId = null;
    this._previewLine = null;
    this._cursor = null;
  }

  activate() {
    this._phase = 0;
    this._startNodeId = null;
    this.vp.canvas.style.cursor = 'crosshair';
    this._cursor = this.vp.snapping.getCursorMesh();
    this.vp.addTempObject(this._cursor);
    this._makePreviewLine();
    eventBus.emit('status:hint', 'Click start node or point. Then click end node or point.');
  }

  _makePreviewLine() {
    const geo  = new THREE.BufferGeometry().setFromPoints([new THREE.Vector3(), new THREE.Vector3()]);
    const mat  = new THREE.LineDashedMaterial({ color: 0x60a5fa, dashSize: 0.2, gapSize: 0.1 });
    this._previewLine = new THREE.Line(geo, mat);
    this._previewLine.computeLineDistances();
    this._previewLine.visible = false;
    this.vp.addTempObject(this._previewLine);
  }

  deactivate() {
    this._phase = 0; this._startNodeId = null;
    this.vp.canvas.style.cursor = 'default';
    this.vp.clearTempObjects();
    this._previewLine = null; this._cursor = null;
  }

  onClick(e) {
    const snap = this.vp.snapping.snap(e);
    if (!snap) return;

    if (this._phase === 0) {
      // Snap to existing node or create new one
      let nodeId = snap.nodeId;
      if (!nodeId) {
        const n = appState.addNode(snap.x, snap.y, snap.z);
        nodeId = n.id;
      }
      this._startNodeId = nodeId;
      this._phase = 1;
      const startNode = appState.model.nodes.get(nodeId);
      if (this._previewLine) this._previewLine.visible = true;
      eventBus.emit('cursor:moved', snap);
    } else {
      // End point
      let nodeId = snap.nodeId;
      if (!nodeId) {
        const n = appState.addNode(snap.x, snap.y, snap.z);
        nodeId = n.id;
      }
      if (nodeId !== this._startNodeId) {
        appState.addElement(this._startNodeId, nodeId);
      }
      // Chain: start next from this node
      this._startNodeId = nodeId;
      eventBus.emit('cursor:moved', snap);
    }
  }

  onMouseMove(e) {
    const snap = this.vp.snapping.snap(e);
    if (!snap) return;
    if (this._cursor) this._cursor.position.set(snap.x, snap.y, snap.z);

    if (this._phase === 1 && this._previewLine && this._startNodeId) {
      const sn = appState.model.nodes.get(this._startNodeId);
      if (sn) {
        const pts = [new THREE.Vector3(sn.x, sn.y, sn.z), new THREE.Vector3(snap.x, snap.y, snap.z)];
        this._previewLine.geometry.setFromPoints(pts);
        this._previewLine.computeLineDistances();
      }
    }
    this.vp.markDirty();
    eventBus.emit('cursor:moved', snap);
  }
}
