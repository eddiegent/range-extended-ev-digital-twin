import { expect, test } from '@playwright/test'

test('renders the scaffolded operator console shell', async ({ page }) => {
  await page.goto('/')

  await expect(
    page.getByRole('heading', { name: 'Range-Extended EV Digital Twin' }),
  ).toBeVisible()

  await expect(page.getByText('Stage 1 scaffolding')).toBeVisible()
})
