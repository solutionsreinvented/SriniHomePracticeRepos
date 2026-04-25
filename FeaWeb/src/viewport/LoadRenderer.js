import * as THREE from 'three';

const MAT_LOAD = new THREE.MeshStandardMaterial({ color: 0xf87171, emissive: 0xf87171, emissiveIntensity: 0.3 });

function makeArrow(dir, origin, length = 1) {
  const group = new THREE.Group();
  const normDir = dir.clone().normalize();
  const shaftLen = length * 0.75;
  const headLen  = length * 0.25;

  // Shaft
  const shaft = new THREE.Mesh(
    new THREE.CylinderGeometry(0.03, 0.03, shaftLen, 8),
    MAT_LOAD.clone()
  );
  shaft.position.copy(normDir.clone().multiplyScalar(shaftLen / 2));
  shaft.quaternion.setFromUnitVectors(new THREE.Vector3(0, 1, 0), normDir);
  group.add(shaft);

  // Head (cone)
  const head = new THREE.Mesh(
    new THREE.ConeGeometry(0.08, headLen, 8),
    MAT_LOAD.clone()
  );
  head.position.copy(normDir.clone().multiplyScalar(shaftLen + headLen / 2));
  head.quaternion.setFromUnitVectors(new THREE.Vector3(0, 1, 0), normDir);
  group.add(head);

  group.position.copy(origin);
  return group;
}

export class LoadRenderer {
  constructor(group, scene) {
    this.group   = group;
    this.scene   = scene;
    this._meshes = new Map();
  }

  refresh(model, settings) {
    this.group.clear();
    this._meshes.clear();
    if (!settings.showLoads) return;

    for (const [id, load] of model.loads) {
      const node = load.nodeId ? model.nodes.get(load.nodeId) : null;
      if (!node) continue;

      const origin = new THREE.Vector3(node.x, node.y, node.z);
      const mag = Math.sqrt(load.Fx ** 2 + load.Fy ** 2 + load.Fz ** 2) || 1;
      const scale = Math.min(Math.max(mag / 10, 0.5), 2.5);
      const dir = new THREE.Vector3(
        load.Fx / mag, load.Fy / mag, load.Fz / mag
      );

      const arrow = makeArrow(dir, origin, scale);
      arrow.userData = { type: 'load', id };
      this._meshes.set(id, arrow);
      this.group.add(arrow);
    }
  }

  getMeshes() { return [...this._meshes.values()]; }
}
