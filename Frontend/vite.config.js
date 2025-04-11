import {defineConfig} from 'vite'
import vue from '@vitejs/plugin-vue'
import fs from 'fs'
import path from 'path';

export default defineConfig({
    plugins: [vue()],
    server: {
        port: 5173,
        strictPort: true,
        historyApiFallback: true,
        https: {
            key: fs.readFileSync('C:/Users/nicth/Documents/GitHub/GET_Prepared/Backup_oppgaver/BackupFiles/Finn-klone/localhost-key.pem'),
            cert: fs.readFileSync('C:/Users/nicth/Documents/GitHub/GET_Prepared/Backup_oppgaver/BackupFiles/Finn-klone/localhost.pem')
        },
        proxy: {
            '/api': {
                target: 'https://localhost:5205',
                changeOrigin: true,
                secure: false
            },
            '/chatHub': {
                target: 'https://localhost:5205',
                changeOrigin: true,
                secure: false,
                ws: true,
            },
        }
    },
    resolve: {
        alias: {
            '@': path.resolve(__dirname, './src'),
        },
    }
})
