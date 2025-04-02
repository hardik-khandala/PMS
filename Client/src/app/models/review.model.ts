export interface SelfReview{
    startDate: string,
    endDate: string,
    selfRating: string,
    strength: string,
    improvement: string,
    criteriaId: number
}

export interface SelfReviewList{
    reviewId: number,
    managerName: string,
    startDate: string,
    endDate: string,
    title: string,
    selfRating: string,
    managerRating: string,
    managerFeedback: string,
    status: string,
    createdAt: string
}

export interface managerReview {
    reviewId: number,
    managerRating: string,
    managerFeedback: string
}

export interface ManagerReviewList {
    reviewId: number,
    empName: string,
    startDate: string,
    endDate: string,
    title: string,
    selfRating: string,
    strength: string,
    improvement: string,
    createdAt: string
}