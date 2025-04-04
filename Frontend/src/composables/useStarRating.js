export function useStarRating() {

    function getStars(rating) {
        const solidStars = '<i class="fa-solid fa-star" style="color: #FFD43B;"></i>'.repeat(rating);
        const emptyStars = '<i class="fa-regular fa-star"></i>'.repeat(5 - rating);
        return solidStars + emptyStars;
    }

    return {getStars};
}