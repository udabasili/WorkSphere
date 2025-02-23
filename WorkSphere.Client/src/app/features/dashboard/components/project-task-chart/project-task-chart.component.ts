import {Component, Input, OnChanges, OnInit} from '@angular/core';
import {ChartConfiguration} from 'chart.js';
import {Team} from '../../../team-managment/model/team';

@Component({
  selector: 'app-project-task-chart',
  standalone: false,

  templateUrl: './project-task-chart.component.html',
  styleUrl: './project-task-chart.component.css'
})
export class ProjectTaskChartComponent implements OnInit, OnChanges {

  @Input() teams: Team[] = []; // Expecting an array of Project objects

  public barChartLegend = true;
  public barChartPlugins: any = [];


  public barChartLabels: ChartConfiguration<'bar'>['data']['labels'] = [];

  public barChartData: ChartConfiguration<'bar'>['data'] = null;

  public barChartOptions: ChartConfiguration<'bar'>['options'] = {
    responsive: false,
    scales: {
      x: {
        ticks: {
          color: 'white',
          font: {
            size: 16
          }
        }
      },
      y: {
        ticks: {
          color: 'white',
          font: {
            size: 15
          }
        }
      }
    },
    //legend styling
    plugins: {
      legend: {
        labels: {
          color: 'white',
          font: {
            size: 15
          }
        }
      }
    }
  };

  constructor() {
  }

  ngOnInit(): void {
    this.updateChartData();
  }

  ngOnChanges(): void {
    this.updateChartData();
  }

  updateChartData() {

    console.log(this.teams);
    this.barChartLabels = this.teams.map(team => team.projectName.slice(0, 10));
    this.barChartData = {
      labels: this.barChartLabels,
      datasets: [
        {label: 'Pending Task', data: this.teams.map(team => team.numOfPendingTasks)},
        {label: 'Completed Task', data: this.teams.map(team => team.numOfCompletedTasks)},
      ]
    }
  }
}
