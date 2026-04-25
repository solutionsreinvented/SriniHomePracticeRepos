/**
 * Sparse direct solver using Cholesky-like LDLᵀ decomposition
 * Operates on dense arrays (suitable for structures up to ~2000 DOFs)
 */

/** Solve K·u = F  (modifies K in-place via Gaussian elimination) */
export function solveLinear(K, F) {
  const n = F.length;
  const A = K.map(row => Float64Array.from(row));
  const b = Float64Array.from(F);

  for (let col = 0; col < n; col++) {
    // Partial pivot
    let maxVal = Math.abs(A[col][col]), maxRow = col;
    for (let row = col + 1; row < n; row++) {
      if (Math.abs(A[row][col]) > maxVal) { maxVal = Math.abs(A[row][col]); maxRow = row; }
    }
    if (maxVal < 1e-12) continue; // singular / constrained DOF
    if (maxRow !== col) { [A[col], A[maxRow]] = [A[maxRow], A[col]]; [b[col], b[maxRow]] = [b[maxRow], b[col]]; }

    // Elimination
    const pivot = A[col][col];
    for (let row = col + 1; row < n; row++) {
      const factor = A[row][col] / pivot;
      if (factor === 0) continue;
      for (let k = col; k < n; k++) A[row][k] -= factor * A[col][k];
      b[row] -= factor * b[col];
    }
  }

  // Back substitution
  const x = new Float64Array(n);
  for (let i = n - 1; i >= 0; i--) {
    let sum = b[i];
    for (let j = i + 1; j < n; j++) sum -= A[i][j] * x[j];
    x[i] = Math.abs(A[i][i]) > 1e-12 ? sum / A[i][i] : 0;
  }
  return x;
}
