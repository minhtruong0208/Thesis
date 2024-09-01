/* eslint-disable */
<template>
  <div class="index-people-holder">
    <div
      v-for="person in persons"
      :key="person.name"
      :class="getAttendanceClass(person)"
      class="name"
    >
      {{ person.name }}
    </div>
  </div>
</template>
  
  <script>
  export default {
    name: 'AttendanceList',
    props: {
      persons: {
        type: Array,
        required: true
      }
    },
    methods: {
      getAttendanceClass(person) {
        if (person.attendanceStatus === 'unknown') {
          return 'name-unknown'
        } else if (person.appearancePercentage >= 80) {
          return 'name-high'
        } else if (person.appearancePercentage >= 50) {
          return 'name-medium'
        } else {
          return 'name-low'
        }
      }
    }
  }
  </script>
  
  <style scoped>
  .index-people-holder {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  align-items: flex-start;
  justify-content: flex-start;
  gap: 27px 40px;
  font-size: 18px;
  }
  
  .name {
    border-radius: 10px;
    height: 52px;
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    padding: 4px 49px;
    box-sizing: border-box;
    color: #fffbfa;
  }
  
  .name-high {
    background-color: #1fd61b;
  }
  
  .name-medium {
    background-color: #d1b317;
  }
  
  .name-low {
    background-color: #342e37;
  }
  .name-unknown {
  background-color: #ff0000;
}
  </style>