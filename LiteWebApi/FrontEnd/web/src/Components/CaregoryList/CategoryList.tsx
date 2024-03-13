import {useEffect, useState} from 'react';
import {getFilteredCategoriesAsync} from '../../Shared/api.ts';
import AddCategoryForm from './AddCategoryForm.tsx';
import {Button, Form, Input, Row, Spin} from 'antd';
import CategoryItem from './CategoryItem.tsx';
import ICategory from '../../Interfaces/Api/ICategory.tsx';
import Pagination from '../Shared/Pagination.tsx';
import styles from '../../styles.module.css';
import {AlignLeftOutlined, PlusCircleOutlined} from "@ant-design/icons";
import {useNavigate} from "react-router-dom";
import {navigateAndSetParams} from "../Shared/NavigationExtensions.ts";
import queryString from "query-string";
import {useAppSelector} from "../../Redux/Hooks/hooks.ts";

interface ICategoryListState {
    showAddCategoryForm: boolean,
    showCategoriesFilter: boolean,

    itemsPerPage: number,
    categories: ICategory[],
    isItemsLoading: boolean,
    availableCategories: number
}

const CategoryList = () => {
    const navigate = useNavigate();
    const queryParams = queryString.parse(window.location.search);
    const {categoryNameFilter, categoryOffset: offset} = queryParams;
    const offsetNumber = Number(offset ?? '0');
    const {user} = useAppSelector(store => store.auth);

    const [state, setState] = useState<ICategoryListState>({
        showAddCategoryForm: false,
        showCategoriesFilter: false,

        itemsPerPage: 3,
        categories: [],
        isItemsLoading: false,
        availableCategories: 0,
    } as ICategoryListState);

    const [nameFilter, setNameFilter] = useState<string>(categoryNameFilter ?? '');

    useEffect(() => {
        loadCategories();
    }, [state.itemsPerPage]);

    const setFilters = () => {
        navigateAndSetParams(navigate, '/', {
            categoryNameFilter: nameFilter,
            categoryOffset: undefined
        });
    }

    const clearFilters = () => {
        navigateAndSetParams(navigate, '/', {
            categoryNameFilter: undefined
        });
    }

    const setOffset = (offset: number) => navigateAndSetParams(navigate, '/', {
        categoryOffset: offset
    });

    const loadCategories = async () => {
        setState((prevState) => ({
            ...prevState,
            isItemsLoading: true
        }));

        try {
            const {itemsPerPage} = state;
            const filteredCategories = await getFilteredCategoriesAsync({
                offset,
                limit: itemsPerPage,
                name: categoryNameFilter,
            });
            const {filteredCategories: categories, availableCategories} = filteredCategories;

            setState((prevState) => ({
                ...prevState,
                categories,
                availableCategories
            }));
        } catch (error: any) {
            setState((prevState) => ({
                ...prevState,
                categories: []
            }));

            console.log("Items loading error");
        }

        setState((prevState) => ({
            ...prevState,
            isItemsLoading: false
        }));
    };

    const addCategoryMenuSwitch = () => {
        setState((prevState) => ({
            ...prevState,
            showAddCategoryForm: !prevState.showAddCategoryForm,
            showCategoriesFilter: false
        }));
    };

    const categoriesFilterSwitch = () => {
        setState((prevState) => ({
            ...prevState,
            showCategoriesFilter: !prevState.showCategoriesFilter,
            showAddCategoryForm: false
        }));
    };

    const hideAddCategoryMenuAndReloadPage = () => {
        setState((prevState) => ({
            ...prevState,
            showAddCategoryForm: false
        }));
        loadCategories();
    };

    const {
        showAddCategoryForm,
        categories,
        isItemsLoading,
        availableCategories,
        itemsPerPage,
        showCategoriesFilter,
    } = state;

    const categoriesHtml = categories.map((c) => (
        <CategoryItem key={c.id} category={c} reloadCategories={loadCategories}/>
    ));

    return (
        <>
            <Row className={`${styles.topMenu}`}>
                {user?.hasRole("Admin") &&
                    (<Button type="primary" onClick={addCategoryMenuSwitch}>
                        <PlusCircleOutlined/>
                        <span>Create category menu</span>
                    </Button>)
                }

                <Button type="primary" onClick={categoriesFilterSwitch}>
                    <AlignLeftOutlined/>
                    <span>Filters</span>
                </Button>
            </Row>

            {showAddCategoryForm && <AddCategoryForm hide={hideAddCategoryMenuAndReloadPage}/>}

            {showCategoriesFilter && (
                <div className={`${styles.darkBorder} ${styles.padding12}`}>
                    <Form
                        name="filterForm"
                        layout="vertical"
                    >
                        <Form.Item label="Name" name="name" initialValue={nameFilter}>
                            <Input placeholder="Enter name" onChange={e => setNameFilter(e.target.value)}/>
                        </Form.Item>

                        <Form.Item>
                            <Button type="primary" onClick={setFilters}>
                                Filter
                            </Button>
                        </Form.Item>

                        <Form.Item>
                            <Button type="primary" onClick={clearFilters}>
                                Clear
                            </Button>
                        </Form.Item>
                    </Form>
                </div>
            )}

            <Pagination maxPagesOnSides={5} itemsPerPage={itemsPerPage}
                        availableQuantity={availableCategories} offset={offsetNumber} setOffset={setOffset}/>

            {/*<div style={{overflow: "hidden"}}>*/}
            <Spin spinning={isItemsLoading} size="large">
                <Row gutter={8} style={{minHeight: 300}}>{categoriesHtml}</Row>
            </Spin>
            {/*</div>*/}
        </>
    );
};

export default CategoryList;
