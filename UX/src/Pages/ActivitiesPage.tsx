import React, { useState, useEffect } from 'react';
import GetActivities from '../components/Activities/GetActivities';
import CreateActivity from '../components/Activities/CreateActivity'; 
import CustomModal from '../components/CustomModal';
import domEventBus from "../utils/domEventBus";

const ActivitiesPage: React.FC = () => {

    const [showModal, setShowModal] = useState(false);
 
    const [refreshTrigger, setRefreshTrigger] = useState(false);  

    useEffect(() => {
        const handleActivityEvent = (e: Event) => {
            const { id } = (e as CustomEvent).detail || {};
            console.log("Activity event triggered for:", id);
            setRefreshTrigger(prev => !prev); 
        };

        domEventBus.addEventListener("activityCreated", handleActivityEvent);
        domEventBus.addEventListener("activityUpdated", handleActivityEvent);

        return () => {
            domEventBus.removeEventListener("activityCreated", handleActivityEvent);
            domEventBus.removeEventListener("activityUpdated", handleActivityEvent);
        };
    }, []);

    return (
        <main className="container py-4">
            <button
                onClick={() => setShowModal(true)}
                className="bg-blue-600 text-white px-4 py-2 rounded mb-3"
            >
               Create Activity
            </button>

            <GetActivities refreshTrigger={refreshTrigger} />

            {showModal && (
                <CustomModal title="Create Activity" body={<CreateActivity onCreated={() => setShowModal(false)} />} isOpen={showModal} onClose={() => setShowModal(false)} />
            )} 
            
        </main>
    );
};

export default ActivitiesPage;
