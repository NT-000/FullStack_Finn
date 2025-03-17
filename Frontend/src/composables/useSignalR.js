import { ref, onMounted, onUnmounted } from 'vue';
import * as signalR from '@microsoft/signalr';

export function useSignalR() {
    const notifications = ref([]);
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub")
        .build();

    const startConnection = async () => {
        try {
            await connection.start();
            console.log("SignalR Connected.");
        } catch (err) {
            console.error("SignalR Connection Error: ", err);
        }
    };

    onMounted(() => {
        startConnection();

        connection.on("ReceiveNotification", (message) => {
            notifications.value.push(message);
        });
    });

    onUnmounted(() => {
        connection.stop();
    });

    return { notifications };
}