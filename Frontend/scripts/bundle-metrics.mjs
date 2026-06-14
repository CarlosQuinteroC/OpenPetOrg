import { gzipSync } from 'node:zlib'
import { mkdir, readFile, stat, writeFile } from 'node:fs/promises'
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

function resolveEntryScript(indexHtml) {
  const match = indexHtml.match(/<script[^>]*type="module"[^>]*src="([^"]+)"[^>]*><\/script>/i)
  if (!match?.[1]) {
    throw new Error('Could not resolve module entry script from dist/index.html')
  }

  return match[1]
}

export async function collectBundleMetrics({
  label,
  distDir = 'dist',
}) {
  const indexHtmlPath = path.resolve(distDir, 'index.html')
  const indexHtml = await readFile(indexHtmlPath, 'utf8')
  const entryAssetPath = resolveEntryScript(indexHtml)
  const normalizedEntryPath = entryAssetPath.replace(/^\//, '')
  const absoluteEntryPath = path.resolve(distDir, normalizedEntryPath)

  const [entryBuffer, entryStats] = await Promise.all([
    readFile(absoluteEntryPath),
    stat(absoluteEntryPath),
  ])

  const gzipBytes = gzipSync(entryBuffer).length

  return {
    label,
    entryFile: normalizedEntryPath,
    rawBytes: entryStats.size,
    gzipBytes,
    generatedAt: new Date().toISOString(),
  }
}

async function run() {
  const args = parseArgs(process.argv.slice(2))
  const label = args.label
  const outPath = args.out ?? path.join('.bundle-metrics', `${label}.json`)
  const distDir = args.dist ?? 'dist'

  if (!label) {
    throw new Error('Missing required --label argument (for example: --label baseline)')
  }

  const metrics = await collectBundleMetrics({ label, distDir })
  const absoluteOutPath = path.resolve(outPath)

  await mkdir(path.dirname(absoluteOutPath), { recursive: true })
  await writeFile(absoluteOutPath, `${JSON.stringify(metrics, null, 2)}\n`, 'utf8')

  process.stdout.write(`${JSON.stringify(metrics, null, 2)}\n`)
}

const currentFilePath = fileURLToPath(import.meta.url)
const invokedPath = process.argv[1] ? path.resolve(process.argv[1]) : ''
const isCliInvocation = invokedPath === currentFilePath

if (isCliInvocation) {
  run().catch((error) => {
    const message = error instanceof Error ? error.message : 'Unknown error while generating bundle metrics'
    process.stderr.write(`${message}\n`)
    process.exit(1)
  })
}
