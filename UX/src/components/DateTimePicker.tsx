import React from 'react';
import { Form } from 'react-bootstrap';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';

interface Props {
    label: string;
    name: string;
    value: string | null | undefined;
    onChange: (name: string, value: string | null) => void;
    required?: boolean;
}

const DateTimePicker: React.FC<Props> = ({ label, name, value, onChange, required }) => {
    const toDate = (val: string | null | undefined) => (val ? new Date(val) : null);

    return (
        <Form.Group className="mb-3">
            <Form.Label className="pe-5">{label}</Form.Label>
            <DatePicker
                selected={toDate(value)}
                onChange={(date: Date | null) => onChange(name, date ? date.toISOString() : null)}
                showTimeSelect
                dateFormat="yyyy-MM-dd HH:mm"
                className="form-control"
                placeholderText={`Select ${label.toLowerCase()}`}
                required={required}
            />
        </Form.Group>
    );
};

export default DateTimePicker;
