angular.module("api-setting-service", [])
.service("ApiSettingService", function(){

    // Application base url.
    this.applicationLink = "";

    // Url of getting personal objective.
    this.getPersonalObjectiveLink = this.applicationLink + '/' + 'assets/data/main-objective.json';

    // Url of getting professional summary.
    this.getProfessionalSummaryLink = this.applicationLink + '/' + 'assets/data/professional-summary.json';

    // Url of getting certificates.
    this.getCertificateLink = this.applicationLink + '/' + 'assets/data/certificate.json';

    // Url of getting personal skills.
    this.getPersonalSkillsLink = this.applicationLink + '/' + 'assets/data/skills.json';

    // Url of getting projects list.
    this.getProjectsLink = this.applicationLink + '/' + 'assets/data/projects-list.json';
});