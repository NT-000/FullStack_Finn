import {ref} from 'vue'

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
        'Dyr',
        'Annet',
    ])
    const conditions = ref([
        'Ny',
        'Nesten ny',
        'Brukt',
        'Godt brukt'
    ])

    return {categories, conditions}
}