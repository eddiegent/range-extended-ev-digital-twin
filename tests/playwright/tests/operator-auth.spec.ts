import { expect, test } from '@playwright/test'

test('shows signed-out shell before authentication', async ({ page }) => {
  await page.route('**/api/auth/status', async (route) => {
    await route.fulfill({
      json: {
        isAuthenticated: false,
        email: null,
        userName: null,
      },
    })
  })

  await page.goto('/')

  await expect(page.getByRole('heading', { name: 'Operator access' })).toBeVisible()
  await expect(page.getByLabel('Email')).toBeVisible()
  await expect(page.getByLabel('Password')).toBeVisible()
  await expect(page.getByText('Demo operator')).toBeVisible()
})

test('signs in and signs out through the operator shell', async ({ page }) => {
  await page.route('**/api/auth/status', async (route) => {
    await route.fulfill({
      json: {
        isAuthenticated: false,
        email: null,
        userName: null,
      },
    })
  })

  await page.route('**/api/auth/sign-in', async (route) => {
    await route.fulfill({
      json: {
        isAuthenticated: true,
        email: 'operator@local.test',
        userName: 'demo.operator',
      },
    })
  })

  await page.route('**/api/auth/sign-out', async (route) => {
    await route.fulfill({
      status: 204,
    })
  })

  await page.goto('/')

  await page.getByLabel('Email').fill('operator@local.test')
  await page.getByLabel('Password').fill('Passw0rd')
  await page.getByRole('button', { name: 'Sign in' }).click()

  await expect(page.getByText('Signed in as operator@local.test')).toBeVisible()
  await expect(page.getByRole('button', { name: 'Sign out' })).toBeVisible()

  await page.getByRole('button', { name: 'Sign out' }).click()

  await expect(page.getByRole('button', { name: 'Sign in' })).toBeVisible()
})
