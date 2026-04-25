import * as THREE from 'three';

const MAT_FIXED  = new THREE.MeshStandardMaterial({ color: 0xfb923c, emissive: 0xfb923c, emissiveIntensity: 0.25, roughness: 0.5 });
const MAT_PINNED = new THREE.MeshStandardMaterial({ color: 0xfb923c, emissive: 0xfb923c, emissiveIntensity: 0.15, roughness: 0.6 });

function makeFixedSymbol() {
  const group = new THREE.Group();
  // Box base
  const base = new THREE.Mesh(new THREE.BoxGeometry(0.25, 0.06, 0.25), MAT_FIXED.clone());
  base.position.y = -0.12;
  group.add(base);
  // Hatching lines
  for (let i = -2; i <= 2; i++) {
    const line = new THREE.Mesh(
      new THREE.BoxGeometry(0.02, 0.001, 0.25),
      new THREE.MeshBasicMaterial({ color: 0xfb923c })
    );
    line.position.set(i * 0.06, -0.15, 0);
    group.add(line);
  }
  return group;
}

function makePinnedSymbol() {
  const group = new THREE.Group();
  // Triangle
  const shape = new THREE.Shape();
  shape.moveTo(0, 0);
  shape.lineTo(-0.15, -0.24);
  shape.lineTo(0.15, -0.24);
  shape.closePath();
  const geo = new THREE.ShapeGeometry(shape);
  const mesh = new THREE.Mesh(geo, MAT_PINNED.clone());
  mesh.rotation.x = -Math.PI / 2; // lay flat → then rotate
  // Use extruded thin prism
  const extGeo = new THREE.ExtrudeGeometry(shape, { depth: 0.04, bevelEnabled: false });
  const tri    = new THREE.Mesh(extGeo, MAT_PINNED.clone());
  tri.rotation.x = Math.PI / 2;
  tri.position.set(0, -0.02, 0.02);
  group.add(tri);
  // Ground line
  const line = new THREE.Mesh(new THREE.BoxGeometry(0.34, 0.04, 0.04), MAT_PINNED.clone());
  line.position.y = -0.26;
  group.add(line);
  return group;
}

export class SupportRenderer {
  constructor(group, scene) {
    this.group   = group;
    this.scene   = scene;
    this._meshes = new Map();
  }

  refresh(model) {
    const existing = new Set(this._meshes.keys());

    for (const [id, sup] of model.supports) {
      existing.delete(id);
      const node = model.nodes.get(sup.nodeId);
      if (!node) continue;

      if (this._meshes.has(id)) {
        this.group.remove(this._meshes.get(id));
        this._meshes.delete(id);
      }

      const sym = sup.type === 'fixed' ? makeFixedSymbol() : makePinnedSymbol();
      sym.position.set(node.x, node.y, node.z);
      sym.userData = { type: 'support', id };
      this._meshes.set(id, sym);
      this.group.add(sym);
    }

    for (const id of existing) {
      this.group.remove(this._meshes.get(id));
      this._meshes.delete(id);
    }
  }

  getMeshes() { return [...this._meshes.values()]; }
}
