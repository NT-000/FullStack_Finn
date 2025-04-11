export function useStarRating() {

    function getStars(rating) {
        const solidPip = '<img src="/f1.png" style="height: 40px; width:40px">'.repeat(rating);
        const emptyPip = '<img src="/f4.png" style="color: white; height: 40px; width:40px">'.repeat(5 - rating);
        return solidPip + emptyPip;
    }

    return {getStars};
}