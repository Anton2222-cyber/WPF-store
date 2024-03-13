import ICategory from "./ICategory"

export default interface IFilteredCategories {
  filteredCategories: ICategory[],
  availableCategories: number
}