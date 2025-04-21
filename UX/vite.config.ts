import { defineConfig } from 'vite'
import plugin from '@vitejs/plugin-react'

export default defineConfig({
    plugins: [plugin()],
    server: {
        port: 30754,
        proxy: {
            '/api': {
                target: 'http://localhost:5122',
                changeOrigin: true,
                rewrite: path => path.replace(/^\/api/, '') 
            }
        }
    }
})
