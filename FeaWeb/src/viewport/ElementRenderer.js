import * as THREE from 'three';

const MAT_BEAM = new THREE.MeshStandardMaterial({
  color: 0x60a5fa, emissive: 0x1d4ed8, emissiveIntensity: 0.15,
  roughness: 0.5, metalness: 0.7
});
const MAT_BEAM_SEL = new THREE.MeshStandardMaterial({
  color: 0xfacc15, emissive: 0xfacc15, emissiveIntensity: 0.4,
  roughness: 0.3, metalness: 0.6
});

function makeBeamMesh(n1, n2) {
  const start = new THREE.Vector3(n1.x, n1.y, n1.z);
  const end   = new THREE.Vector3(n2.x, n2.y, n2.z);
  const dir   = end.clone().sub(start);
  const len   = dir.length();
  if (len < 1e-6) return null;

  const geo  = new THREE.CylinderGeometry(0.04, 0.04, len, 8, 1);
  const mesh = new THREE.Mesh(geo, MAT_BEAM.clone());
  const mid  = start.clone().lerp(end, 0.5);
  mesh.position.copy(mid);
  mesh.quaternion.setFromUnitVectors(
    new THREE.Vector3(0, 1, 0),
    dir.normalize()
  );
  return mesh;
}

export class ElementRenderer {
  constructor(group, scene) {
    this.group   = group;
    this.scene   = scene;
    this._meshes = new Map(); // elementId → mesh
  }

  refresh(model) {
    const existing = new Set(this._meshes.keys());

    for (const [id, el] of model.elements) {
      existing.delete(id);
      const n1 = model.nodes.get(el.node1Id);
      const n2 = model.nodes.get(el.node2Id);
      if (!n1 || !n2) continue;

      // Remove old mesh to rebuild (handles node moves)
      if (this._meshes.has(id)) {
        const old = this._meshes.get(id);
        this.group.remove(old);
        old.geometry.dispose();
      }

      const mesh = makeBeamMesh(n1, n2);
      if (!mesh) continue;
      mesh.userData = { type: 'element', id };
      mesh.castShadow = true;
      this._meshes.set(id, mesh);
      this.group.add(mesh);
    }

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
    mesh.material = selected ? MAT_BEAM_SEL.clone() : MAT_BEAM.clone();
  }

  getMeshes() { return [...this._meshes.values()]; }
}
