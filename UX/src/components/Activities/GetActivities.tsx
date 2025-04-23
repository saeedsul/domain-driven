import React, { useEffect, useState } from "react";
import { format } from "date-fns";
import Timeline from "../Timeline";

import { Activity, getAllActivities } from "../../api/activity/activityApi";
import { handleRequest } from "../../utils/requestWrapper";
import Engagement from "../Engagement";

const groupByMonth = (activities: Activity[]): Record<string, Activity[]> => {
    return activities.reduce((acc, act) => {
        const monthKey = format(new Date(act.createdDate), "MMMM yyyy");
        if (!acc[monthKey]) acc[monthKey] = [];
        acc[monthKey].push(act);
        return acc;
    }, {} as Record<string, Activity[]>);
};

const GetActivities: React.FC<{ refreshTrigger: boolean }> = ({ refreshTrigger }) => {
    const [activities, setActivities] = useState<Activity[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [openSections, setOpenSections] = useState<Record<string, boolean>>({});

    useEffect(() => {
        loadActivities();
    }, [refreshTrigger]); 

    const loadActivities = async () => {
        setLoading(true);
        const [data, err] = await handleRequest({ promise: getAllActivities() }); console.log(data);
        if (err) {
            console.error(err);
            setError("Failed to load activities");
        } else if (data) {
            const sorted = data.sort(
                (a: Activity, b: Activity) => new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime()
            );
            setActivities(sorted);
        }
        setLoading(false);
    };


    const toggleSection = (month: string) => {
        setOpenSections((prev) => ({
            ...prev,
            [month]: !prev[month],
        }));
    };
     

    const grouped = groupByMonth(activities);

    return (
        <div className="accordion">
            {loading ? (
                <p>Loading...</p>
            ) : error ? (
                <p className="text-danger">{error}</p>
            ) : (
                Object.entries(grouped).map(([month, items]) => {
                    const isOpen = openSections[month] ?? false;
                   
                    return (
                        <div key={month} className="accordion-item mb-2 rounded">

                            <div className="accordion-header">
                                <div className="d-flex w-100 align-items-center border-bottom">
                                 
                                    <div className="flex-grow-1" style={{ flexBasis: "50%" }}>
                                        <h2 className="fw-bolder m-2">{month}</h2>
                                    </div>
                                     
                                    <div className="d-flex justify-content-center mt-3" style={{ flexBasis: "45%" }}>
                                        <Engagement activities={items} />                                        
                                    </div>
                                     
                                    <div className="d-flex justify-content-end" style={{ flexBasis: "5%" }}>                                       
                                        <button
                                            className={`accordion-button ${!isOpen ? "collapsed" : "bg-transparent"}`}
                                            type="button"
                                            onClick={() => toggleSection(month)}
                                        > 
                                        </button>
                                    </div>
                                </div>
                            </div>  
                            {isOpen && (
                                <div className="accordion-body">
                                    <Timeline activities={items} />
                                </div>
                            )}
                        </div>
                    );
                })
            )}
        </div>
    );
};

export default GetActivities;
