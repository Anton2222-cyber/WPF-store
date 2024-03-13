import styles from "../../../styles.module.css";
import {Button, Row} from "antd";
import {LeftOutlined} from "@ant-design/icons";
import {useNavigate, useParams} from "react-router-dom";
import {navigateAndSaveParams} from "../../Shared/NavigationExtensions.ts";

function ProductPage() {
    const {categoryId: categoryIdString} = useParams();
    const navigate = useNavigate();

    const backUrl = `/category/${categoryIdString}`;

    return (
        <>
            <Row className={`${styles.topMenu}`}>
                <Button type="primary" onClick={() => navigateAndSaveParams(navigate, backUrl)}>
                    <LeftOutlined/>
                    <span>Return</span>
                </Button>
            </Row>
        </>
    );
}

export default ProductPage;