import {getBasketItemsAsync} from "../../Shared/api.ts";
import {useEffect, useState} from "react";
import IBasketItem from "../../Interfaces/Api/IBasketItem.ts";
import {BasketItem} from "./BasketItem.tsx";
import {Col, Button, message, Spin} from "antd";
import {useNavigate} from "react-router-dom";


function Basket() {
    const [processing, setProcessing] = useState(false);
    const [basketItems, setBasketItems] = useState<IBasketItem[]>([]);
    const navigate = useNavigate();
    const [messageApi, contextHolder] = message.useMessage();

    const toOrdering = () => navigate(`/ordering`);

    const loadBasketItems = async () => {
        setProcessing(true);

        try {
            const items = await getBasketItemsAsync();
            setBasketItems(items);
        } catch (error: any) {
            messageApi.error('Error');
        }

        setProcessing(false);
    }

    useEffect(() => {
        loadBasketItems();
    }, []);


    const productCards = basketItems
        .map((bp) => (
            <BasketItem key={bp.id} basketProduct={bp} reloadItems={loadBasketItems}/>
        ));

    return (
        <>
            {contextHolder}

            <Spin spinning={processing}>
                <Col align="middle">
                    {productCards.length !== 0
                        ? <>
                            {productCards}
                            <Button type="primary" onClick={toOrdering} style={{width: "33%", marginTop: "20px"}}>Buy
                                now</Button>
                        </>
                        : <p style={{fontSize: "40px", textAlign: "center", marginTop: "60px"}}>Basket is empty</p>}
                </Col>
            </Spin>
        </>
    );
}

export default Basket;