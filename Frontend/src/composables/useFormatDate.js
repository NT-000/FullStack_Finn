export function useDateFormat() {
    const formatDate = (dateStr) => {
        return new Date(dateStr).toLocaleString(navigator.language,{
            day: "2-digit",
            month: "long",
            year: "numeric",
            hour: "2-digit",
            minute: "2-digit"
        });
    };

    return {formatDate};
}