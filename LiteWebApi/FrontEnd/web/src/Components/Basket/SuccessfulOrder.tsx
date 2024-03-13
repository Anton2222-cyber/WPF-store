import {CheckCircleOutlined} from "@ant-design/icons";
import {Link} from "react-router-dom";

function SuccessfulOrder() {
    return (
        <div style={{display: 'flex', justifyContent: 'center', alignItems: 'center', marginTop: '100px'}}>
            <div style={{display: 'flex', flexDirection: 'column', alignItems: 'center'}}>
                <CheckCircleOutlined style={{color: 'green', fontSize: '250px'}}/>
                <div style={{color: 'green', paddingTop: '20px', fontSize: '30px'}}>Successful order</div>
                <Link to='/' style={{fontSize: '20px', paddingTop: '20px'}}>Go to main</Link>
            </div>
        </div>
    );
}

export default SuccessfulOrder;