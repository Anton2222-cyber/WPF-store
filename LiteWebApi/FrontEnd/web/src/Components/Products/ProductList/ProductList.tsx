import React, {useEffect, useState} from 'react';
import {getFilteredProductsAsync} from '../../../Shared/api.ts';
import {Button, Form, Input, InputNumber, Row, Spin} from 'antd';
import {useNavigate, useParams} from 'react-router-dom';
import IProduct from "../../../Interfaces/Api/IProduct.ts";
import ProductItem from "./ProductItem.tsx";
import {AlignLeftOutlined, LeftOutlined, PlusCircleOutlined} from "@ant-design/icons";
import styles from '../../../styles.module.css';
import {navigateAndSaveParams, navigateAndSetParams} from "../../Shared/NavigationExtensions.ts";
import queryString from "query-string";
import Pagination from "../../Shared/Pagination.tsx";
import {useAppSelector} from "../../../Redux/Hooks/hooks.ts";

interface IProductListState {
    products: IProduct[],
    isItemsLoading: boolean
}

const ProductList = () => {
    const navigate = useNavigate();
    const {categoryId: categoryIdString} = useParams();
    const categoryId = Number.parseInt(categoryIdString!);
    const createProductUrl = `/category/${categoryIdString}/create-product`;
    const thisUrl = `/category/${categoryIdString}`;
    const categoriesUrl = `/`;
    const [showFilter, setShowFilter] = useState(false);
    const itemsPerPage = 3;
    const {user} = useAppSelector(store => store.auth);

    const queryParams = queryString.parse(window.location.search);
    const {
        productNameFilter,
        productDescriptionFilter,
        productMinPriceFilter,
        productMaxPriceFilter,
        productOffset: offset
    } = queryParams;

    const offsetNumber = Number(offset ?? '0');
    const [nameFilter, setNameFilter] = useState<string>(productNameFilter ?? '');
    const [descriptionFilter, setDescriptionFilter] = useState<string>(productDescriptionFilter ?? '');
    const [minPriceFilter, setMinPriceFilter] = useState<number | null>(+productMinPriceFilter);
    const [maxPriceFilter, setMaxPriceFilter] = useState<number | null>(+productMaxPriceFilter);
    const [availableQuantity, setAvailableQuantity] = useState<number>(0);

    const [state, setState] = useState<IProductListState>({
        products: [],
        isItemsLoading: false
    } as IProductListState);

    useEffect(() => {
        loadProducts(categoryId);
    }, [categoryId]);

    const productsFilterSwitch = () => setShowFilter(!showFilter);

    const setOffset = (offset: number) => navigateAndSetParams(navigate, thisUrl, {
        productOffset: offset
    })

    const setFilters = () => {
        navigateAndSetParams(navigate, thisUrl, {
            productNameFilter: nameFilter,
            productDescriptionFilter: descriptionFilter,
            productMinPriceFilter: minPriceFilter,
            productMaxPriceFilter: maxPriceFilter,
            productOffset: undefined
        });
    }

    const clearFilters = () => {
        navigateAndSetParams(navigate, thisUrl, {
            productNameFilter: undefined,
            productDescriptionFilter: undefined,
            productMinPriceFilter: undefined,
            productMaxPriceFilter: undefined
        });
    }

    const loadProducts = async (categoryId: number) => {
        setState((prevState) => ({
            ...prevState,
            isItemsLoading: true
        }));

        try {
            const result = await getFilteredProductsAsync({
                categoryId,
                name: productNameFilter,
                description: productDescriptionFilter,
                minPrice: productMinPriceFilter,
                maxPrice: productMaxPriceFilter,

                limit: itemsPerPage,
                offset
            });

            setAvailableQuantity(result.availableQuantity);

            setState((prevState) => ({
                ...prevState,
                products: result.filteredProducts,
            }));
        } catch (error: any) {
            setState((prevState) => ({
                ...prevState,
                products: []
            }));

            console.log("Items loading error");
        }

        setState((prevState) => ({
            ...prevState,
            isItemsLoading: false
        }));
    };

    const {
        products,
        isItemsLoading
    } = state;

    const productsHtml = products.map((p) => (
        <ProductItem key={p.id} product={p}/>
    ));

    return (
        <>
            <Row className={`${styles.topMenu}`}>
                <Button type="primary" onClick={() => navigateAndSaveParams(navigate, categoriesUrl)}>
                    <LeftOutlined/>
                    <span>To categories</span>
                </Button>

                {user?.hasRole("Admin") &&
                    (<Button type="primary" onClick={() => navigateAndSaveParams(navigate, createProductUrl)}>
                        <PlusCircleOutlined/>
                        <span>Create new product</span>
                    </Button>)
                }

                <Button type="primary" onClick={productsFilterSwitch}>
                    <AlignLeftOutlined/>
                    <span>Filters</span>
                </Button>
            </Row>

            {showFilter && (
                <div className={`${styles.darkBorder} ${styles.padding12}`}>
                    <Form
                        name="filterForm"
                        layout="vertical"
                    >
                        <Form.Item label="Name" name="name" initialValue={nameFilter}>
                            <Input placeholder="Enter name" onChange={e => setNameFilter(e.target.value)}/>
                        </Form.Item>

                        <Form.Item label="Description" name="description" initialValue={descriptionFilter}>
                            <Input placeholder="Enter description"
                                   onChange={e => setDescriptionFilter(e.target.value)}/>
                        </Form.Item>

                        <Form.Item label="Min price" name="min price" initialValue={minPriceFilter}>
                            <InputNumber
                                precision={2}
                                style={{
                                    width: '100%'
                                }}
                                onChange={value => setMinPriceFilter(value)}
                            />
                        </Form.Item>

                        <Form.Item label="Max price" name="max price" initialValue={maxPriceFilter}>
                            <InputNumber
                                precision={2}
                                style={{
                                    width: '100%'
                                }}
                                onChange={value => setMaxPriceFilter(value)}
                            />
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
                        availableQuantity={availableQuantity} offset={offsetNumber} setOffset={setOffset}/>

            <Spin spinning={isItemsLoading} size="large">
                <Row gutter={16} style={{minHeight: 300, maxWidth: '100%'}}>{productsHtml}</Row>
            </Spin>
        </>
    );
};

export default ProductList;
