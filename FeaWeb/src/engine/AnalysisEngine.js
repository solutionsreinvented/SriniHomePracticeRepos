import { beamLocalStiffness, transformationMatrix, globalStiffness } from './BeamStiffness.js';
import { solveLinear } from './MatrixSolver.js';

const NDOF = 6; // DOFs per node

export class AnalysisEngine {
  analyze(model, loadCaseId) {
    const nodes    = [...model.nodes.values()];
    const elements = [...model.elements.values()];
    const supports = [...model.supports.values()];

    const nNodes  = nodes.length;
    const totalDOF = nNodes * NDOF;
    if (nNodes === 0 || elements.length === 0) throw new Error('Model has no nodes or members.');

    // --- DOF mapping: nodeId → starting DOF index ---
    const nodeIndex = new Map();
    nodes.forEach((n, i) => nodeIndex.set(n.id, i * NDOF));

    // --- Global stiffness matrix & force vector ---
    const K = Array.from({ length: totalDOF }, () => new Float64Array(totalDOF));
    const F = new Float64Array(totalDOF);

    // Assemble element stiffness
    for (const el of elements) {
      const n1 = model.nodes.get(el.node1Id);
      const n2 = model.nodes.get(el.node2Id);
      if (!n1 || !n2) continue;

      const sec = model.sections.get(el.sectionId) || [...model.sections.values()][0];
      const mat = model.materials.get(el.materialId) || [...model.materials.values()][0];
      if (!sec || !mat) continue;

      const dx = n2.x-n1.x, dy = n2.y-n1.y, dz = n2.z-n1.z;
      const L  = Math.sqrt(dx*dx + dy*dy + dz*dz);
      if (L < 1e-9) continue;

      const Kl = beamLocalStiffness(mat.E, mat.G, sec.A, sec.Iy, sec.Iz, sec.J, L);
      const T  = transformationMatrix(n1, n2);
      const Kg = globalStiffness(Kl, T);

      const i0 = nodeIndex.get(n1.id);
      const i1 = nodeIndex.get(n2.id);
      const dofs = [...Array(6).keys()].map(k => i0+k).concat([...Array(6).keys()].map(k => i1+k));

      for (let a = 0; a < 12; a++)
        for (let b = 0; b < 12; b++)
          K[dofs[a]][dofs[b]] += Kg[a][b];
    }

    // Assemble loads for the requested load case
    for (const load of model.loads.values()) {
      if (load.loadCaseId !== loadCaseId) continue;
      if (load.type === 'node-force' && load.nodeId) {
        const i0 = nodeIndex.get(load.nodeId);
        if (i0 === undefined) continue;
        F[i0]   += load.Fx;
        F[i0+1] += load.Fy;
        F[i0+2] += load.Fz;
        F[i0+3] += load.Mx;
        F[i0+4] += load.My;
        F[i0+5] += load.Mz;
      }
    }

    // Apply boundary conditions (penalty method)
    const LARGE = 1e20;
    for (const sup of supports) {
      const i0 = nodeIndex.get(sup.nodeId);
      if (i0 === undefined) continue;
      const restraints = [sup.ux, sup.uy, sup.uz, sup.rx, sup.ry, sup.rz];
      restraints.forEach((r, k) => {
        if (r) { K[i0+k][i0+k] += LARGE; }
      });
    }

    // Solve
    const U = solveLinear(K, Array.from(F));

    // Extract results
    const displacements = {};
    for (const n of nodes) {
      const i0 = nodeIndex.get(n.id);
      displacements[n.id] = {
        ux: U[i0], uy: U[i0+1], uz: U[i0+2],
        rx: U[i0+3], ry: U[i0+4], rz: U[i0+5],
      };
    }

    // Reactions
    const reactions = {};
    for (const sup of supports) {
      const i0 = nodeIndex.get(sup.nodeId);
      if (i0 === undefined) continue;
      // R = K_full · U − F  (only restrained DOFs matter)
      // Simplified: reaction = K_penalty_diag * U
      const d = displacements[sup.nodeId];
      reactions[sup.nodeId] = {
        Rx: sup.ux ? -d.ux * LARGE : 0,
        Ry: sup.uy ? -d.uy * LARGE : 0,
        Rz: sup.uz ? -d.uz * LARGE : 0,
      };
    }

    // Member forces (axial only, simplified)
    const memberForces = {};
    for (const el of elements) {
      const n1 = model.nodes.get(el.node1Id);
      const n2 = model.nodes.get(el.node2Id);
      if (!n1 || !n2) continue;
      const d1 = displacements[n1.id];
      const d2 = displacements[n2.id];
      const sec = model.sections.get(el.sectionId) || [...model.sections.values()][0];
      const mat = model.materials.get(el.materialId) || [...model.materials.values()][0];
      if (!sec || !mat) continue;
      const dx = n2.x-n1.x, dy = n2.y-n1.y, dz = n2.z-n1.z;
      const L  = Math.sqrt(dx*dx + dy*dy + dz*dz);
      const lx = dx/L, ly = dy/L, lz = dz/L;
      // Axial elongation
      const delta = (d2.ux-d1.ux)*lx + (d2.uy-d1.uy)*ly + (d2.uz-d1.uz)*lz;
      const N = mat.E * sec.A * delta / L;
      memberForces[el.id] = { N, Vy: 0, Vz: 0, T: 0, My1: 0, My2: 0, Mz1: 0, Mz2: 0 };
    }

    return { displacements, reactions, memberForces, loadCaseId };
  }
}
