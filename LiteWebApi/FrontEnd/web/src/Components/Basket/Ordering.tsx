import {Button, Card, Col, message, Select, Spin} from "antd";
import Meta from "antd/es/card/Meta";
import IArea from "../../Interfaces/Api/IArea.ts";
import {useEffect, useState} from "react";
import {
    getAreasAsync,
    getBasketTotalPriceAsync,
    getPostOfficesAsync,
    getSettlementsAsync,
    orderAsync
} from "../../Shared/api.ts";
import ISettlement from "../../Interfaces/Api/ISettlement.ts";
import IPostOffice from "../../Interfaces/Api/IPostOffice.ts";
import {MapContainer, TileLayer, Marker, Popup} from 'react-leaflet';
import 'leaflet/dist/leaflet.css';
import {useNavigate} from "react-router-dom";

export function Ordering() {
    const [messageApi, contextHolder] = message.useMessage();
    const navigate = useNavigate();

    const [processing, setProcessing] = useState(false);

    const [totalPrice, setTotalPrice] = useState(0);

    const [areas, setAreas] = useState<IArea[]>([]);
    const [settlements, setSettlements] = useState<ISettlement[]>([]);
    const [postOffices, setPostOffices] = useState<IPostOffice[]>([]);

    const [selectedAreaValue, setSelectedAreaValue] = useState<number | null>(null);
    const [selectedSettlementValue, setSelectedSettlementValue] = useState<number | null>(null);
    const [selectedPostOfficeValue, setSelectedPostOfficeValue] = useState<number | null>(null);

    const [position, setPosition] = useState<[number, number]>([0, 0]);
    const [postOfficeName, setPostOfficeName] = useState<string>('');

    useEffect(() => {
        loadAreas();
        loadBasketTotalPrice();
    }, []);

    const loadBasketTotalPrice = async () => {
        setProcessing(true);

        try {
            const totalPrice = await getBasketTotalPriceAsync();
            setTotalPrice(totalPrice.totalPrice);
        } catch (error: any) {
            messageApi.error('Error loading total price');
        }

        setProcessing(false);
    }

    const loadAreas = async () => {
        setProcessing(true);

        try {
            const areas = await getAreasAsync();
            setAreas(areas);
        } catch (error: any) {
            setAreas([]);
            messageApi.error('Error loading areas');
        }

        setProcessing(false);
    }

    const loadSettlements = async (areaId: number) => {
        setProcessing(true);

        try {
            const settlements = await getSettlementsAsync(areaId);
            setSettlements(settlements);
        } catch (error: any) {
            setSettlements([]);
            messageApi.error('Error loading settlements');
        }

        setProcessing(false);
    }

    const loadPostOffices = async (settlementId: number) => {
        setProcessing(true);

        try {
            const postOffices = await getPostOfficesAsync(settlementId);
            setPostOffices(postOffices);
        } catch (error: any) {
            setPostOffices([]);
            messageApi.error('Error loading post offices');
        }

        setProcessing(false);
    }

    const areaOptions = areas.map((area) => {
        return {
            value: area.id,
            label: area.name
        }
    })

    const settlementOptions = settlements.map((settlement) => {
        return {
            value: settlement.id,
            label: settlement.name
        }
    })

    const postOfficesOptions = postOffices.map((postOffice) => {
        return {
            value: postOffice.id,
            label: postOffice.name
        }
    })

    const areaChanged = (id: number) => {
        setSelectedAreaValue(id);
        settlementChanged(null);
        if (id != null) {
            loadSettlements(id);
        } else {
            setSettlements([]);
        }
    }

    const settlementChanged = (id: number | null) => {
        setSelectedSettlementValue(id);
        postOfficeChanged(null);

        if (id != null) {
            loadPostOffices(id);
        } else {
            setPostOffices([]);
        }
    }

    const postOfficeChanged = (id: number | null) => {
        setSelectedPostOfficeValue(id);

        if (id != null) {
            const selectedPostOffice = postOffices.filter(po => po.id === id)[0];

            setPosition([selectedPostOffice.latitude, selectedPostOffice.longitude]);
            setPostOfficeName(selectedPostOffice.name);
        }
    }

    const order = async () => {
        if (!selectedPostOfficeValue) {
            messageApi.info('Post office is not selected!');
            return;
        }

        setProcessing(true);

        const form = new FormData();
        form.append('PostOfficeId', selectedPostOfficeValue.toString());

        try {
            await orderAsync(form);
            navigate('/successful-order');
        } catch (error: any) {
            messageApi.error('Error ordering');
        }

        setProcessing(false);
    }

    return (
        <>
            {contextHolder}

            <Spin spinning={processing}>
                <Col align="middle">
                    <Card>
                        <Meta title="Total price"/>
                        <p>{totalPrice} UAH</p>
                    </Card>

                    <Card>
                        <Meta title="Address"/>

                        <h4 style={{paddingTop: '20px'}}>Area</h4>
                        <Select
                            value={selectedAreaValue}
                            style={{width: 500}}
                            onChange={areaChanged}
                            options={areaOptions}
                        />

                        <h4 style={{paddingTop: '20px'}}>Settlement</h4>
                        <Select
                            value={selectedSettlementValue}
                            style={{width: 500}}
                            onChange={settlementChanged}
                            options={settlementOptions}
                        />

                        <h4 style={{paddingTop: '20px'}}>Post office</h4>
                        <Select
                            value={selectedPostOfficeValue}
                            style={{width: 500}}
                            onChange={postOfficeChanged}
                            options={postOfficesOptions}
                        />
                    </Card>

                    {selectedPostOfficeValue &&
                        <MapContainer position={position} center={position} zoom={14} scrollWheelZoom={true} style={{
                            width: '800px',
                            height: '500px',
                            border: '2px solid rgba(0, 0, 0, 0.25)',
                            borderRadius: '10px',
                            marginTop: '20px'
                        }}>
                            <TileLayer
                                attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                            />
                            <Marker position={position}>
                                <Popup>{postOfficeName}</Popup>
                            </Marker>
                        </MapContainer>}

                    <Button type="primary" style={{marginTop: '20px', width: '500px'}} onClick={order}>Order</Button>
                </Col>
            </Spin>
        </>
    );
}