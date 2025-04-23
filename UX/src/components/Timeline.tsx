import React, { useState } from "react"; 
import { MailCheck, Eye, Ban, Pencil } from "lucide-react";
import { format } from "date-fns";
import { Activity, UpdateActivityI} from "../api/activity/activityApi";
import UpdateActivity from "../components/Activities/UpdateActivity";   
import CustomModal from "./CustomModal";

interface TimelineProps {
    activities: Activity[];
}

const hasDate = (date?: string | null) => {
    const parsedDate = date ? new Date(date) : null;
    return parsedDate && !isNaN(parsedDate.getTime());
};

const Timeline: React.FC<TimelineProps> = ({ activities }) => {
    const [expandedIds, setExpandedIds] = useState<string[]>([]);
    const [selectedActivity, setSelectedActivity] = useState<UpdateActivityI>();
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);

    const toggleDetails = (id: string) => {
        setExpandedIds((prev) =>
            prev.includes(id) ? prev.filter((i) => i !== id) : [...prev, id]
        );
    };

    const handleEdit = (activity: Activity) => {
        const mapped: UpdateActivityI = {
            id: activity.id,
            name: activity.name,
            fromAddress: activity.fromAddress ?? "",
            toEmailAddress: activity.toEmailAddress,
            fromName: activity.fromName,
            openedDate: activity.openedDate,
            bouncedDate: activity.bouncedDate,
        };
        setSelectedActivity(mapped);
        setIsModalOpen(true);
    }; 


    return (
        <div className="slds-timeline__item_expandable slds-timeline__item_event">
            <ul className="slds-timeline">
                {activities.map((activity) => {
                    const isExpanded = expandedIds.includes(activity.id);

                    return (
                        <div key={activity.id} className="ml-n2">
                            <li className="slds-timeline__item mb-3">
                                <div className="slds-media">
                                    <div className="slds-media__figure">
                                        <button
                                            className="slds-button slds-button_icon slds-m-right_xx-small"
                                            onClick={() => toggleDetails(activity.id)}
                                            aria-expanded={isExpanded}
                                            title="Toggle details"
                                        >
                                            <i className={`bi ${isExpanded ? "bi-chevron-down" : "bi-chevron-right"}`}></i>
                                        </button>

                                        <div className="slds-icon_container slds-icon-standard-email slds-timeline__icon" title="email">
                                            <svg className="slds-icon slds-icon_small" aria-hidden="true">
                                                <use xlinkHref="/icons/standard-sprite/svg/symbols.svg#email"></use>
                                            </svg>
                                        </div>
                                    </div>

                                    <div className="slds-media__body">
                                        <div className="slds-grid slds-grid_align-spread slds-timeline__trigger">
                                            <div className="slds-grid slds-grid_vertical-align-center slds-size_6-of-12 slds-no-space">
                                                <h3 className="slds-truncate">
                                                    <strong>{activity.name}</strong>
                                                </h3>
                                            </div>

                                            <div className="slds-col slds-truncate slds-size_2-of-12 slds-text-align_center">
                                                {hasDate(activity.sentDate) && (
                                                    <span
                                                        className="slds-icon_container slds-icon-utility-email slds-m-around_xx-small text-success" 
                                                        title={`Sent: ${format(new Date(activity.sentDate!), "dd/MM/yyyy H:mm")}`}

                                                    >
                                                        <span className="slds-icon slds-icon_x-small"><MailCheck /></span>
                                                    </span>
                                                )}
                                                {hasDate(activity.openedDate) && (
                                                    <span
                                                        className="slds-icon_container slds-icon-utility-preview slds-m-around_xx-small text-success"                                                      
                                                        title={`Opened: ${format(new Date(activity.openedDate!), "dd/MM/yyyy H:mm")}`}
                                                    >
                                                        <span className="slds-icon slds-icon_x-small"><Eye /></span>
                                                    </span>
                                                )}
                                                {hasDate(activity.bouncedDate) && (
                                                    <span
                                                        className="slds-icon_container slds-icon-utility-preview slds-m-around_xx-small text-danger"
                                                        title={`Bounced: ${format(new Date(activity.bouncedDate!), "dd/MM/yyyy H:mm")}`}
                                                    >
                                                        <span className="slds-icon slds-icon_x-small"><Ban /></span>
                                                    </span>
                                                )}
                                            </div>

                                            <div className="slds-col slds-truncate slds-size_2-of-12 slds-timeline__actions slds-timeline__actions_inline">
                                                <p className="slds-timeline__date"> {format(new Date(activity.sentDate), "dd/MM/yyyy H:mm")}</p>
                                            </div>

                                            <div className="slds-col slds-truncate slds-timeline__actions slds-timeline__actions_inline">
                                                <button
                                                    type="button"
                                                    className="slds-button slds-button_icon slds-button_icon-border-filled"
                                                    title="Edit Activity"
                                                    onClick={() => handleEdit(activity)}
                                                >
                                                    <Pencil size={16} />
                                                    <span className="slds-assistive-text">Edit Activity</span>
                                                </button>
                                            </div>
                                        </div>

                                        {isExpanded && (
                                            <article
                                                id={activity.id}
                                                className="slds-box slds-theme_shade slds-m-top_x-small slds-m-horizontal_xx-small timeline-details"
                                            >
                                                <div className="slds-grid slds-wrap">
                                                    <div className="slds-size--1-of-2 slds-p-vertical--xx-small forceListRecordItem">
                                                        <div className="slds-truncate recordCell recordCellLabel slds-text-color_weak slds-text-title">
                                                            From Address
                                                        </div>
                                                        <div className="slds-item_detail slds-truncate recordCell recordCellDetail">
                                                            <ul className="addressList emailuiAddressListOutput">
                                                                <li className="truncate addressItem">
                                                                    {activity.fromName}
                                                                </li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                    <div className="slds-size--1-of-2 slds-p-vertical--xx-small forceListRecordItem">
                                                        <div className="slds-truncate recordCell recordCellLabel slds-text-color_weak slds-text-title">
                                                            To Address
                                                        </div>
                                                        <div className="slds-item_detail slds-truncate recordCell recordCellDetail">
                                                            <ul className="addressList emailuiAddressListOutput">
                                                                <li className="truncate addressItem">
                                                                    {activity.toEmailAddress}
                                                                </li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                    <div className="slds-size_2-of-2 slds-var-p-vertical_xx-small">
                                                        <div
                                                            title="Text Body"
                                                            className="slds-text-title slds-truncate"
                                                        >
                                                            Text Body
                                                        </div>
                                                    </div>
                                                </div>
                                            </article>
                                        )}
                                    </div>
                                </div>
                            </li>
                        </div>
                    );
                })}
            </ul>

            {isModalOpen && (
                <CustomModal title="Update Activity" body={<UpdateActivity activity={selectedActivity} onUpdated={() => setIsModalOpen(false)} />} isOpen={isModalOpen} onClose={() => setIsModalOpen(false)}/> 
            )}
        </div>
    );
};

export default Timeline;
