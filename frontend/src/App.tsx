import { useEffect, useMemo, useState } from 'react'
import type { FormEvent } from 'react'

type AuthStatus = {
  isAuthenticated: boolean
  email: string | null
  userName: string | null
}

const signedOutAuthStatus: AuthStatus = {
  isAuthenticated: false,
  email: null,
  userName: null,
}

const demoOperator = {
  email: 'operator@local.test',
  password: 'Passw0rd',
}

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL ?? '/api'

async function readAuthStatus(): Promise<AuthStatus> {
  const response = await fetch(`${apiBaseUrl}/auth/status`, {
    credentials: 'include',
  })

  if (!response.ok) {
    throw new Error('Unable to read operator session.')
  }

  return (await response.json()) as AuthStatus
}

function App() {
  const [authStatus, setAuthStatus] = useState<AuthStatus | null>(null)
  const [email, setEmail] = useState(demoOperator.email)
  const [password, setPassword] = useState(demoOperator.password)
  const [submitError, setSubmitError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  useEffect(() => {
    let isMounted = true

    readAuthStatus()
      .then((status) => {
        if (isMounted) {
          setAuthStatus(status)
        }
      })
      .catch(() => {
        if (isMounted) {
          setSubmitError('Unable to reach the operator session endpoint.')
          setAuthStatus(signedOutAuthStatus)
        }
      })

    return () => {
      isMounted = false
    }
  }, [])

  const authMessage = useMemo(() => {
    if (!authStatus) {
      return 'Checking operator session...'
    }

    if (authStatus.isAuthenticated && authStatus.email) {
      return `Signed in as ${authStatus.email}`
    }

    return 'No operator session is active.'
  }, [authStatus])

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setIsSubmitting(true)
    setSubmitError(null)

    try {
      const response = await fetch(`${apiBaseUrl}/auth/sign-in`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        credentials: 'include',
        body: JSON.stringify({ email, password }),
      })

      if (!response.ok) {
        setSubmitError('Sign-in failed. Check the demo credentials and try again.')
        return
      }

      const nextStatus = (await response.json()) as AuthStatus
      setAuthStatus(nextStatus)
    } catch {
      setSubmitError('Unable to sign in right now.')
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleSignOut() {
    setIsSubmitting(true)
    setSubmitError(null)

    try {
      const response = await fetch(`${apiBaseUrl}/auth/sign-out`, {
        method: 'POST',
        credentials: 'include',
      })

      if (!response.ok) {
        setSubmitError('Unable to sign out right now.')
        return
      }

      setAuthStatus(signedOutAuthStatus)
    } catch {
      setSubmitError('Unable to sign out right now.')
    } finally {
      setIsSubmitting(false)
    }
  }

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

        <section className="grid gap-6 lg:grid-cols-[1.1fr,0.9fr]">
          <article className="rounded-3xl border border-slate-800 bg-slate-900/70 p-6">
            <div className="flex items-start justify-between gap-4">
              <div>
                <h2 className="text-xl font-semibold">Operator access</h2>
                <p className="mt-2 text-slate-300">{authMessage}</p>
              </div>
              <span
                className={`rounded-full px-3 py-1 text-sm font-medium ${
                  authStatus?.isAuthenticated
                    ? 'bg-emerald-400/20 text-emerald-200'
                    : 'bg-amber-400/20 text-amber-100'
                }`}
              >
                {authStatus?.isAuthenticated ? 'Authenticated' : 'Signed out'}
              </span>
            </div>

            {submitError ? (
              <p className="mt-4 rounded-2xl border border-rose-400/40 bg-rose-500/10 px-4 py-3 text-sm text-rose-100">
                {submitError}
              </p>
            ) : null}

            {authStatus?.isAuthenticated ? (
              <div className="mt-6 space-y-4">
                <div className="rounded-2xl border border-emerald-400/30 bg-emerald-500/10 p-4">
                  <p className="text-sm text-emerald-100">
                    Operator session is ready for protected simulation controls.
                  </p>
                  <dl className="mt-3 space-y-2 text-sm text-slate-200">
                    <div>
                      <dt className="text-slate-400">Email</dt>
                      <dd>{authStatus.email}</dd>
                    </div>
                    <div>
                      <dt className="text-slate-400">User name</dt>
                      <dd>{authStatus.userName}</dd>
                    </div>
                  </dl>
                </div>
                <button
                  type="button"
                  onClick={handleSignOut}
                  disabled={isSubmitting}
                  className="rounded-xl bg-slate-100 px-4 py-2 text-sm font-semibold text-slate-950 transition hover:bg-white disabled:cursor-not-allowed disabled:opacity-60"
                >
                  Sign out
                </button>
              </div>
            ) : (
              <form className="mt-6 space-y-4" onSubmit={handleSubmit}>
                <div className="rounded-2xl border border-cyan-400/30 bg-cyan-400/10 p-4 text-sm text-cyan-50">
                  <p className="font-semibold">Demo operator</p>
                  <p className="mt-2">Email: {demoOperator.email}</p>
                  <p>Password: {demoOperator.password}</p>
                </div>

                <label className="block text-sm text-slate-200">
                  <span className="mb-2 block">Email</span>
                  <input
                    value={email}
                    onChange={(event) => setEmail(event.target.value)}
                    type="email"
                    className="w-full rounded-xl border border-slate-700 bg-slate-950 px-4 py-3 text-slate-50 outline-none ring-0 transition focus:border-cyan-300"
                  />
                </label>

                <label className="block text-sm text-slate-200">
                  <span className="mb-2 block">Password</span>
                  <input
                    value={password}
                    onChange={(event) => setPassword(event.target.value)}
                    type="password"
                    className="w-full rounded-xl border border-slate-700 bg-slate-950 px-4 py-3 text-slate-50 outline-none ring-0 transition focus:border-cyan-300"
                  />
                </label>

                <button
                  type="submit"
                  disabled={isSubmitting || authStatus === null}
                  className="rounded-xl bg-cyan-300 px-4 py-2 text-sm font-semibold text-slate-950 transition hover:bg-cyan-200 disabled:cursor-not-allowed disabled:opacity-60"
                >
                  Sign in
                </button>
              </form>
            )}
          </article>

          <article className="rounded-3xl border border-slate-800 bg-slate-900/70 p-6">
            <h2 className="text-xl font-semibold">Scaffold scope</h2>
            <ul className="mt-4 space-y-3 text-slate-300">
              <li>React + TypeScript + Tailwind foundation</li>
              <li>Ready for realtime streaming via SignalR</li>
              <li>Bootstrap operator sign-in and sign-out flow</li>
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
