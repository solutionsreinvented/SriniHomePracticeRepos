/**
 * 3D Euler-Bernoulli Beam Element — 12×12 local stiffness matrix
 * DOF order per node: [ux, uy, uz, rx, ry, rz]
 */
export function beamLocalStiffness(E, G, A, Iy, Iz, J, L) {
  const EAL  = E * A / L;
  const EIy  = E * Iy;
  const EIz  = E * Iz;
  const GJL  = G * J / L;
  const L2   = L * L;
  const L3   = L * L * L;

  // Build 12×12 using standard beam stiffness formulation
  const K = Array.from({ length: 12 }, () => new Float64Array(12));

  // Axial
  K[0][0]  =  EAL;  K[0][6]  = -EAL;
  K[6][0]  = -EAL;  K[6][6]  =  EAL;

  // Torsion
  K[3][3]  =  GJL;  K[3][9]  = -GJL;
  K[9][3]  = -GJL;  K[9][9]  =  GJL;

  // Bending in x-y plane (Iz — about z-axis)
  const a = 12 * EIz / L3, b = 6 * EIz / L2, c = 4 * EIz / L, d = 2 * EIz / L;
  K[1][1]  =  a;  K[1][5]  =  b;  K[1][7]  = -a;  K[1][11]  =  b;
  K[5][1]  =  b;  K[5][5]  =  c;  K[5][7]  = -b;  K[5][11]  =  d;
  K[7][1]  = -a;  K[7][5]  = -b;  K[7][7]  =  a;  K[7][11]  = -b;
  K[11][1] =  b;  K[11][5] =  d;  K[11][7] = -b;  K[11][11] =  c;

  // Bending in x-z plane (Iy — about y-axis)
  const p = 12 * EIy / L3, q = 6 * EIy / L2, r = 4 * EIy / L, s = 2 * EIy / L;
  K[2][2]  =  p;  K[2][4]  = -q;  K[2][8]  = -p;  K[2][10]  = -q;
  K[4][2]  = -q;  K[4][4]  =  r;  K[4][8]  =  q;  K[4][10]  =  s;
  K[8][2]  = -p;  K[8][4]  =  q;  K[8][8]  =  p;  K[8][10]  =  q;
  K[10][2] = -q;  K[10][4] =  s;  K[10][8] =  q;  K[10][10] =  r;

  return K;
}

/** Transformation matrix T (12×12) from local to global coordinates */
export function transformationMatrix(n1, n2) {
  const dx = n2.x - n1.x, dy = n2.y - n1.y, dz = n2.z - n1.z;
  const L  = Math.sqrt(dx*dx + dy*dy + dz*dz);
  const lx = dx/L, ly = dy/L, lz = dz/L;

  // Local x-axis = beam axis
  // Local y-axis: perpendicular to beam in vertical plane
  let yx, yy, yz;
  if (Math.abs(lx) < 0.9 && Math.abs(lz) < 0.9) {
    // Global Y not parallel to beam
    yx = -ly * lx; yy = 1 - ly * ly; yz = -ly * lz;
  } else {
    yx = 0; yy = lz; yz = -ly;
  }
  const ylen = Math.sqrt(yx*yx + yy*yy + yz*yz);
  yx /= ylen; yy /= ylen; yz /= ylen;

  // Local z-axis = cross product of x and y
  const zx = ly * yz - lz * yy;
  const zy = lz * yx - lx * yz;
  const zz = lx * yy - ly * yx;

  // 3×3 rotation
  const R = [
    [lx, ly, lz],
    [yx, yy, yz],
    [zx, zy, zz],
  ];

  // Build 12×12 block-diagonal
  const T = Array.from({ length: 12 }, () => new Float64Array(12));
  for (let b = 0; b < 4; b++) {
    for (let i = 0; i < 3; i++)
      for (let j = 0; j < 3; j++)
        T[b*3+i][b*3+j] = R[i][j];
  }
  return T;
}

/** K_global = Tᵀ · K_local · T  (12×12) */
export function globalStiffness(Kl, T) {
  const n = 12;
  // Tmp = K_local · T
  const Tmp = Array.from({ length: n }, () => new Float64Array(n));
  for (let i = 0; i < n; i++)
    for (let j = 0; j < n; j++)
      for (let k = 0; k < n; k++)
        Tmp[i][j] += Kl[i][k] * T[k][j];
  // Kg = Tᵀ · Tmp
  const Kg = Array.from({ length: n }, () => new Float64Array(n));
  for (let i = 0; i < n; i++)
    for (let j = 0; j < n; j++)
      for (let k = 0; k < n; k++)
        Kg[i][j] += T[k][i] * Tmp[k][j];
  return Kg;
}
