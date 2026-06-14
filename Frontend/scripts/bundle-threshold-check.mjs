import { mkdir, readFile, writeFile } from 'node:fs/promises'
import path from 'node:path'
import { fileURLToPath } from 'node:url'

function parseArgs(argv) {
  const args = {}

  for (let i = 0; i < argv.length; i += 1) {
    const token = argv[i]
    if (!token.startsWith('--')) {
      continue
    }

    const key = token.slice(2)
    const value = argv[i + 1]
    if (!value || value.startsWith('--')) {
      args[key] = 'true'
      continue
    }

    args[key] = value
    i += 1
  }

  return args
}

function toPercent(deltaBytes, baselineRawBytes) {
  if (baselineRawBytes <= 0) {
    return 0
  }

  return Number(((deltaBytes / baselineRawBytes) * 100).toFixed(2))
}

export function evaluateThreshold({ baselineRawBytes, candidateRawBytes }) {
  const deltaBytes = candidateRawBytes - baselineRawBytes
  const deltaPercent = toPercent(deltaBytes, baselineRawBytes)

  const isUnder500kb = candidateRawBytes < 512_000
  const improvedAtLeast20Percent = deltaPercent <= -20
  const pass = isUnder500kb || improvedAtLeast20Percent

  const reason = pass
    ? isUnder500kb
      ? 'PASS: candidate entry bundle is under 500 kB'
      : 'PASS: candidate entry bundle improved at least 20% versus baseline'
    : 'FAIL: candidate entry bundle did not meet threshold (<500 kB OR >=20% reduction)'

  return {
    baselineRawBytes,
    candidateRawBytes,
    deltaBytes,
    deltaPercent,
    pass,
    reason,
  }
}

async function readMetrics(filePath) {
  const content = await readFile(path.resolve(filePath), 'utf8')
  return JSON.parse(content)
}

async function run() {
  const args = parseArgs(process.argv.slice(2))
  const baselinePath = args.baseline
  const candidatePath = args.candidate
  const candidateLabel = args['candidate-label'] ?? 'candidate'
  const outPath = args.out ?? path.join('.bundle-metrics', `threshold-${candidateLabel}.json`)

  if (!baselinePath || !candidatePath) {
    throw new Error('Missing required args: --baseline <file> --candidate <file>')
  }

  const [baseline, candidate] = await Promise.all([
    readMetrics(baselinePath),
    readMetrics(candidatePath),
  ])

  const evaluated = evaluateThreshold({
    baselineRawBytes: baseline.rawBytes,
    candidateRawBytes: candidate.rawBytes,
  })

  const report = {
    baselineLabel: baseline.label,
    candidateLabel: candidate.label,
    baselineEntryFile: baseline.entryFile,
    candidateEntryFile: candidate.entryFile,
    ...evaluated,
    generatedAt: new Date().toISOString(),
  }

  const absoluteOutPath = path.resolve(outPath)
  await mkdir(path.dirname(absoluteOutPath), { recursive: true })
  await writeFile(absoluteOutPath, `${JSON.stringify(report, null, 2)}\n`, 'utf8')

  process.stdout.write(`${JSON.stringify(report, null, 2)}\n`)

  if (!report.pass) {
    process.exitCode = 1
  }
}

const currentFilePath = fileURLToPath(import.meta.url)
const invokedPath = process.argv[1] ? path.resolve(process.argv[1]) : ''
const isCliInvocation = invokedPath === currentFilePath

if (isCliInvocation) {
  run().catch((error) => {
    const message = error instanceof Error ? error.message : 'Unknown error while checking bundle threshold'
    process.stderr.write(`${message}\n`)
    process.exit(1)
  })
}
