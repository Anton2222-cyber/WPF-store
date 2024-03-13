import styles from '../../styles.module.css';
import {useEffect} from "react";

interface IPagination {
    itemsPerPage: number,
    availableQuantity: number,
    offset: number,
    setOffset: (offset: number) => void,
    maxPagesOnSides: number
}

function Pagination(props: IPagination) {
    const {itemsPerPage, availableQuantity, offset, setOffset, maxPagesOnSides} = props;

    const pagesTotal = Math.ceil(availableQuantity / itemsPerPage);
    const currentPage = Math.floor(offset / itemsPerPage) + 1;

    const setPage = (page: number) => {
        setOffset((page - 1) * itemsPerPage);
    }

    useEffect(() => {
        if (currentPage > pagesTotal && pagesTotal > 0) {
            setPage(pagesTotal);
        }
    }, [currentPage, pagesTotal]);

    const numbers = [];
    for (let i = Math.max(1, currentPage - maxPagesOnSides); i <= Math.min(pagesTotal, currentPage + maxPagesOnSides); i++) {
        numbers.push(i);
    }

    const pages = numbers.map(n => (
        <div
            key={n}
            className={`${styles.page} ${currentPage === n && styles.activePage}`}
            onClick={() => setPage(n)}
        >
      <span>
        {n}
      </span>
        </div>)
    );

    return (
        <div className={styles.pagination}>
            <div className={styles.page} onClick={() => {
                if (currentPage > 1) setPage(currentPage - 1);
            }}>
                <span>←</span>
            </div>
            {pages}
            <div className={styles.page} onClick={() => {
                if (currentPage < pagesTotal) setPage(currentPage + 1);
            }}>
                <span>→</span>
            </div>
        </div>
    );
}

export default Pagination;