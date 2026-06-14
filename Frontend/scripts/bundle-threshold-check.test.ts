import { describe, expect, it } from 'vitest'
import { evaluateThreshold } from './bundle-threshold-check.mjs'

describe('evaluateThreshold', () => {
  it('passes when candidate raw bytes are under 500 kB', () => {
    const report = evaluateThreshold({
      baselineRawBytes: 530_000,
      candidateRawBytes: 499_000,
    })

    expect(report.pass).toBe(true)
    expect(report.reason).toContain('under 500 kB')
  })

  it('passes when candidate improves at least 20%', () => {
    const report = evaluateThreshold({
      baselineRawBytes: 600_000,
      candidateRawBytes: 470_000,
    })

    expect(report.pass).toBe(true)
    expect(report.deltaPercent).toBeLessThanOrEqual(-20)
  })

  it('fails when candidate is over 500 kB and under 20% improvement', () => {
    const report = evaluateThreshold({
      baselineRawBytes: 530_000,
      candidateRawBytes: 520_000,
    })

    expect(report.pass).toBe(false)
    expect(report.reason).toContain('did not meet threshold')
  })
})
