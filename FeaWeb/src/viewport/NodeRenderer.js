import * as THREE from 'three';

const GEO_NODE = new THREE.SphereGeometry(0.08, 12, 8);
const MAT_NODE = new THREE.MeshStandardMaterial({
  color: 0x2dd4bf, emissive: 0x2dd4bf, emissiveIntensity: 0.3,
  roughness: 0.3, metalness: 0.6
});
const MAT_NODE_SEL = new THREE.MeshStandardMaterial({
  color: 0xfacc15, emissive: 0xfacc15, emissiveIntensity: 0.5,
  roughness: 0.2, metalness: 0.7
});

export class NodeRenderer {
  constructor(group, scene) {
    this.group  = group;
    this.scene  = scene;
    this._meshes = new Map(); // nodeId → mesh
  }

  refresh(model) {
    const existing = new Set(this._meshes.keys());

    for (const [id, node] of model.nodes) {
      existing.delete(id);
      if (!this._meshes.has(id)) {
        const mesh = new THREE.Mesh(GEO_NODE, MAT_NODE.clone());
        mesh.userData = { type: 'node', id };
        mesh.castShadow = true;
        this._meshes.set(id, mesh);
        this.group.add(mesh);
      }
      const mesh = this._meshes.get(id);
      mesh.position.set(node.x, node.y, node.z);
    }
    // Remove deleted
    for (const id of existing) {
      const mesh = this._meshes.get(id);
      this.group.remove(mesh);
      mesh.geometry.dispose();
      mesh.material.dispose();
      this._meshes.delete(id);
    }
  }

  setSelected(id, selected) {
    const mesh = this._meshes.get(id);
    if (!mesh) return;
    mesh.material = selected ? MAT_NODE_SEL.clone() : MAT_NODE.clone();
  }

  getMeshes() { return [...this._meshes.values()]; }

  highlightHovered(id) {
    for (const [nid, mesh] of this._meshes) {
      mesh.material.emissiveIntensity = (nid === id) ? 0.8 : 0.3;
    }
  }
}
