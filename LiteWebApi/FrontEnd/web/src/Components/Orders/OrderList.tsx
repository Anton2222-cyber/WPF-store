import {useEffect, useState} from "react";
import {Col, message, Spin, Switch} from "antd";
import {getCustomersOrdersFilteredAsync, getOrdersFilteredAsync} from "../../Shared/api.ts";
import IOrder from "../../Interfaces/Api/IOrder.ts";
import Pagination from "../Shared/Pagination.tsx";
import OrderItem from "./OrderItem.tsx";
import ICustomerOrder from "../../Interfaces/Api/ICustomerOrder.ts";
import {CheckOutlined, CloseOutlined} from "@ant-design/icons";
import {useAppSelector} from "../../Redux/Hooks/hooks.ts";

function OrderList() {
    const [processing, setProcessing] = useState(false);
    const [orderItems, setOrderItems] = useState<IOrder[] | ICustomerOrder[]>([]);
    const [messageApi, contextHolder] = message.useMessage();
    const itemsPerPage = 3;
    const [availableQuantity, setAvailableQuantity] = useState<number>(0);
    const [offset, setOffset] = useState<number>(0);
    const [customersOrdersChecked, setCustomersOrdersChecked] = useState<boolean>(false);
    const {user} = useAppSelector(store => store.auth);

    const loadOrderItems = async () => {
        setProcessing(true);

        try {
            const getFunction = customersOrdersChecked ? getCustomersOrdersFilteredAsync : getOrdersFilteredAsync;

            const response = await getFunction({
                offset,
                limit: itemsPerPage
            });

            setOrderItems(response.filteredOrders);
            setAvailableQuantity(response.availableQuantity)
        } catch (error: any) {
            messageApi.error('Error');
        }

        setProcessing(false);
    }

    useEffect(() => {
        loadOrderItems();
    }, [offset, customersOrdersChecked]);


    const orderCards = orderItems
        .map((o) => (
            <OrderItem key={o.id} order={o}/>
        ));

    return (
        <>
            {contextHolder}

            <Pagination maxPagesOnSides={5} itemsPerPage={itemsPerPage}
                        availableQuantity={availableQuantity} offset={offset} setOffset={setOffset}/>

            {user?.hasRole('Admin') &&
                <div style={{
                    margin: 20,
                }}>
                    <span style={{
                        marginRight: 10
                    }}>Show all customers orders</span>
                    <Switch
                        checked={customersOrdersChecked}
                        checkedChildren={<CheckOutlined/>}
                        unCheckedChildren={<CloseOutlined/>}
                        onChange={setCustomersOrdersChecked}
                    />
                </div>}

            <Spin spinning={processing}>
                <Col align="middle">
                    {orderCards.length !== 0
                        ? <>
                            {orderCards}
                        </>
                        : <p style={{fontSize: "40px", textAlign: "center", marginTop: "60px"}}>No purchases yet</p>}
                </Col>
            </Spin>
        </>
    );
}

export default OrderList;