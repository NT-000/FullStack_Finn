import { ref } from 'vue'

export function useCategories() {
    const categories = ref([
        'Bøker',
        'Elektronikk',
        'Klesplagg',
        'Våpen',
        'Leker',
        'Instrumenter',
        'Bolig',
        'Verktøy',
        'Næring',
        'Kjøretøy',
        'Treningsutstyr',
        'Annet',
    ])

    return { categories }
}