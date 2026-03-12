import { defineConfig } from 'vite'
import path from 'node:path'
import electron from 'vite-plugin-electron/simple'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
    clearScreen: false,
    plugins: [
        react(),
        electron({
            main: {
                // Shortcut of `build.lib.entry`.
                entry: 'electron/main.ts',
            },
            preload: {
                // Shortcut of `build.rollupOptions.input`.
                input: 'electron/preload.ts',
            },
            // Reviewers: this optional can be used if you want to use the Electron Renderer process
            // to facilitate communication with the Main process (IPC)
            renderer: {},
        }),
    ],
    server: {
        proxy: {
            '/api': {
                target: 'http://127.0.0.1:5241',
                changeOrigin: true
            }
        }
    }
})
