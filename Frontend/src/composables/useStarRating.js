import {computed} from "vue";

export function useStarRating(rating) {
return computed(() => {
    const stars = rating;
    console.log("stars",stars);
    const solidStars = '<i class="fa-solid fa-star" style="color: #FFD43B;"></i>'.repeat(stars);
    const emptyStars = '<i class="fa-regular fa-star"></i>'.repeat(5 - stars);
    return solidStars + emptyStars;
})
}