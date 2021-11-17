export interface Pagination {
    currentPage:number;
    itemsPerPage:number;
    totalpages:number;
    totalItems:number;
}

export class PaginationResult<T>{
    result:T;
    pagination:Pagination;

}
