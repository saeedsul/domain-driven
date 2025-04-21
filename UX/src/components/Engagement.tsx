import React from "react";
import { Activity } from "../api/activity/activityApi";

const calculateEngagementLevel = (
    activities: Activity[]
): {
    totalSent: number;
    totalOpened: number;
    level: "Low" | "Medium" | "High";
} => {
    const totalSent = activities.filter((act) => act.sentDate).length;
    const totalOpened = activities.filter((act) => act.openedDate).length;

    const ratio = totalSent > 0 ? totalOpened / totalSent : 0;

    let level: "Low" | "Medium" | "High" = "Low";
    if (ratio > 0.66) level = "High";
    else if (ratio > 0.33) level = "Medium";

    return { totalSent, totalOpened, level };
};

const getTrafficColor = (level: "Low" | "Medium" | "High") => {
    switch (level) {
        case "High":
            return "bg-success";
        case "Medium":
            return "bg-warning";
        case "Low":
        default:
            return "bg-danger";
    }
};

const Engagement: React.FC<{ activities: Activity[] }> = ({ activities }) => {
    const { totalSent, totalOpened, level } = calculateEngagementLevel(activities);
    const colorClass = getTrafficColor(level);

    return (
        <div className="mb-3 d-flex justify-content-between align-items-center">  
            <div
                className={`rounded-circle d-flex align-items-center justify-content-center text-white ${colorClass}`}
                style={{
                    width: "40px",
                    height: "40px",
                    fontSize: "0.75rem",
                    float: "right",
                    cursor: "pointer",
                }}
                title={`📬 Sent: ${totalSent} | 📬 Opened: ${totalOpened} | 🔥 Level: ${level}`}
            >
                <strong>{level}</strong>
            </div>
        </div>

    );
};

export default Engagement;
