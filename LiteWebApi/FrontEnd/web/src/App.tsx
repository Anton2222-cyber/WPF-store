import {BrowserRouter, Route, Routes} from 'react-router-dom';
import CategoryList from './Components/CaregoryList/CategoryList';
import Header from './Components/Header.tsx';
import ProductList from "./Components/Products/ProductList/ProductList.tsx";
import Basket from "./Components/Basket/Basket.tsx";
import {Ordering} from "./Components/Basket/Ordering.tsx";
import SuccessfulOrder from "./Components/Basket/SuccessfulOrder.tsx";
import CreateProduct from "./Components/Products/CreateProduct/CreateProduct.tsx";
import ProductPage from "./Components/Products/ProductPage/ProductPage.tsx";
import EditProduct from "./Components/Products/EditProduct/EditProduct.tsx";
import OrderList from "./Components/Orders/OrderList.tsx";

function App() {
    return (
        <BrowserRouter>
            <Header/>
            <Routes>
                <Route path="/" element={<CategoryList/>}/>
                <Route path="/category/:categoryId" element={<ProductList/>}/>
                <Route path="/category/:categoryId/product/:productId" element={<ProductPage/>}/>
                <Route path="/category/:categoryId/create-product" element={<CreateProduct/>}/>
                <Route path="/category/:categoryId/edit-product/:id" element={<EditProduct/>}/>
                <Route path="/basket" element={<Basket/>}/>
                <Route path="/ordering" element={<Ordering/>}/>
                <Route path="/successful-order" element={<SuccessfulOrder/>}/>
                <Route path="/orders" element={<OrderList/>}/>
            </Routes>
        </BrowserRouter>
    );
}

export default App
