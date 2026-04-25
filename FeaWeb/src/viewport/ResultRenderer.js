import * as THREE from 'three';

const MAT_DEFORM = new THREE.LineBasicMaterial({ color: 0xfacc15, linewidth: 2 });

export class ResultRenderer {
  constructor(group, scene) {
    this.group = group;
    this.scene = scene;
  }

  refresh(model, results, settings) {
    this.group.clear();
    if (!results || !settings.showDeformed) return;
    const scale = settings.deformedScale;

    // Draw deformed shape
    for (const [id, el] of model.elements) {
      const n1 = model.nodes.get(el.node1Id);
      const n2 = model.nodes.get(el.node2Id);
      if (!n1 || !n2) continue;

      const d1 = results.displacements[el.node1Id] || { ux:0, uy:0, uz:0 };
      const d2 = results.displacements[el.node2Id] || { ux:0, uy:0, uz:0 };

      const p1 = new THREE.Vector3(n1.x + d1.ux * scale, n1.y + d1.uy * scale, n1.z + d1.uz * scale);
      const p2 = new THREE.Vector3(n2.x + d2.ux * scale, n2.y + d2.uy * scale, n2.z + d2.uz * scale);

      const geo = new THREE.BufferGeometry().setFromPoints([p1, p2]);
      const line = new THREE.Line(geo, MAT_DEFORM);
      this.group.add(line);
    }
  }
}
