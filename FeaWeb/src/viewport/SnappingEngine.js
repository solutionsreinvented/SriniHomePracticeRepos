import * as THREE from 'three';
import { appState } from '../core/AppState.js';

export class SnappingEngine {
  constructor(viewport) {
    this.vp        = viewport;
    this.gridSize  = 0.5;   // 0.5m grid snap
    this.enabled   = true;
    this._ray      = new THREE.Raycaster();
    this._mouse    = new THREE.Vector2();
    this._nodeSnapDist = 0.4; // world units
  }

  /** Returns snapped world position from mouse event */
  snap(e) {
    const rect = this.vp.canvas.getBoundingClientRect();
    this._mouse.x =  ((e.clientX - rect.left)  / rect.width)  * 2 - 1;
    this._mouse.y = -((e.clientY - rect.top)   / rect.height) * 2 + 1;
    this._ray.setFromCamera(this._mouse, this.vp.camera);

    // 1. Node snap (highest priority)
    const nodeSnap = this._snapToNode();
    if (nodeSnap) return { ...nodeSnap, snapType: 'node' };

    // 2. Ground plane intersection
    const hits = this._ray.intersectObject(this.vp.groundPlane);
    if (!hits.length) return null;
    const pt = hits[0].point;

    if (!this.enabled) return { x: pt.x, y: pt.y, z: pt.z, snapType: 'none' };

    // 3. Grid snap
    const gs = this.gridSize;
    return {
      x: Math.round(pt.x / gs) * gs,
      y: 0,
      z: Math.round(pt.z / gs) * gs,
      snapType: 'grid'
    };
  }

  _snapToNode() {
    const model = appState.model;
    let best = null, bestDist = Infinity;

    for (const [id, node] of model.nodes) {
      const pos  = new THREE.Vector3(node.x, node.y, node.z);
      const dist = this._ray.ray.distanceToPoint(pos);
      if (dist < this._nodeSnapDist && dist < bestDist) {
        bestDist = dist;
        best = { x: node.x, y: node.y, z: node.z, nodeId: id };
      }
    }
    return best;
  }

  /** Create a preview cursor sphere */
  getCursorMesh() {
    if (!this._cursorMesh) {
      this._cursorMesh = new THREE.Mesh(
        new THREE.SphereGeometry(0.06, 8, 6),
        new THREE.MeshBasicMaterial({ color: 0xfacc15, transparent: true, opacity: 0.85 })
      );
      this._cursorMesh.name = '__cursor__';
    }
    return this._cursorMesh;
  }
}
