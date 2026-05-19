function App() {
  return (
    <main className="min-h-screen bg-slate-950 text-slate-50">
      <div className="mx-auto flex min-h-screen max-w-6xl flex-col gap-10 px-6 py-12">
        <header className="space-y-4">
          <p className="text-sm font-semibold uppercase tracking-[0.3em] text-cyan-300">
            Stage 1 scaffolding
          </p>
          <div className="space-y-3">
            <h1 className="text-4xl font-semibold tracking-tight sm:text-5xl">
              Range-Extended EV Digital Twin
            </h1>
            <p className="max-w-3xl text-lg text-slate-300">
              Frontend shell for the operator console, realtime telemetry, event
              timeline, subsystem status, and scenario-driven simulation controls.
            </p>
          </div>
        </header>

        <section className="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
          {[
            'Scenario console shell',
            'SignalR client boundary',
            'Telemetry dashboard shell',
            'Authentication shell',
          ].map((item) => (
            <article
              key={item}
              className="rounded-2xl border border-slate-800 bg-slate-900/60 p-5 shadow-lg shadow-slate-950/30"
            >
              <p className="text-sm text-slate-400">Planned surface</p>
              <h2 className="mt-2 text-lg font-medium text-slate-100">{item}</h2>
            </article>
          ))}
        </section>

        <section className="grid gap-6 lg:grid-cols-[1.4fr,1fr]">
          <article className="rounded-3xl border border-slate-800 bg-slate-900/70 p-6">
            <h2 className="text-xl font-semibold">Scaffold scope</h2>
            <ul className="mt-4 space-y-3 text-slate-300">
              <li>React + TypeScript + Tailwind foundation</li>
              <li>Ready for realtime streaming via SignalR</li>
              <li>Prepared for authenticated operator workflows</li>
              <li>Prepared for Playwright end-to-end coverage</li>
            </ul>
          </article>

          <article className="rounded-3xl border border-cyan-400/30 bg-cyan-400/10 p-6">
            <h2 className="text-xl font-semibold text-cyan-100">Next stage</h2>
            <p className="mt-4 text-slate-200">
              The next implementation phase fills in scenario controls, telemetry
              read models, event history, and subsystem-specific views without
              changing the project structure.
            </p>
          </article>
        </section>
      </div>
    </main>
  )
}

export default App
