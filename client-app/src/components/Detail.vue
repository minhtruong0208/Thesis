<template>
  <IndexLayout>
    <div class="detail">
      <div v-if="!selectedStudent && !editingStudent">
        <div class="search-container">
          <input type="text" v-model="searchTerm" placeholder="Search" />
        </div>
        <table>
          <thead>
            <tr>
              <th>Tên</th>
              <th>Học phần</th>
              <th>Học kỳ</th>
              <th>Số lần hiện diện</th>
              <th>Số lần cảnh báo</th>
              <th>Số lần vắng</th>
              <th>Tỉ lệ nghỉ</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="student in filteredStudents" :key="student.id">
              <td>{{ student.name }}</td>
              <td>{{ classNames[student.classId] }}</td>
              <td>1</td>
              <td>{{ countAttendanceStatus(student, 'present') }}</td>
              <td>{{ countAttendanceStatus(student, 'warning') }}</td>
              <td>{{ countAttendanceStatus(student, 'absent') }}</td>
              <td>{{ calculateAbsenceRate(student) }}%</td>
              <td>
                <button class="action-button" @click="showAttendanceDetails(student)">View</button>
                <button class="action-button" @click="openEditStudentForm(student)">Edit</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div v-else-if="selectedStudent">
        <button class="back-button" @click="goBack">&#8592; Quay lại</button>
        <h3>Chi tiết điểm danh của {{ selectedStudent.name }}</h3>
        <table>
          <thead>
            <tr>
              <th>Ngày học</th>
              <th>Trạng thái</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="record in selectedStudent.attendanceRecords" :key="record.date">
              <td>{{ record.date }}</td>
              <td>{{ record.status }}</td>
            </tr>
          </tbody>
        </table>
      </div>

      <div v-else>
        <button class="back-button" @click="cancelEdit">&#8592; Quay lại</button>
        <h3>Chỉnh sửa thông tin của sinh viên</h3>
        <form class="edit-form" @submit.prevent="updateStudent">
          <div>
            <label>Name:</label>
            <input type="text" v-model="editingStudent.name" required />
          </div>
          <div>
            <label>Class ID:</label>
            <input type="text" v-model="editingStudent.classId" required />
          </div>
          <div>
            <h4>Attendance Records</h4>
            <table>
              <thead>
                <tr>
                  <th>Date</th>
                  <th>Status</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(record, index) in editingStudent.attendanceRecords" :key="index">
                  <td>
                    <input type="date" v-model="record.date" required />
                  </td>
                  <td>
                    <select v-model="record.status" required>
                      <option value="present">Present</option>
                      <option value="warning">Warning</option>
                      <option value="absent">Absent</option>
                    </select>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <button class="action-button" type="submit">Save</button>
        </form>
      </div>
    </div>
  </IndexLayout>
</template>

<script>
import { ref, computed, onMounted } from 'vue'
import IndexLayout from './IndexLayout.vue'
import axios from 'axios'

export default {
  /* eslint-disable vue/multi-word-component-names */
  name: 'Detail',
  components: {
    IndexLayout
  },
  setup() {
    const students = ref([])
    const searchTerm = ref('')
    const selectedStudent = ref(null)
    const editingStudent = ref(null)
    const classNames = ref({});
    
    const fetchClassNames = async () => {
      for (const student of students.value) {
        try {
          const response = await axios.get(`http://localhost:5016/api/home/classes/${student.classId}`);
          classNames.value[student.classId] = response.data.className;
        } catch (error) {
          console.error(`Error fetching class name for class ID ${student.classId}:`, error);
        }
      }
    };

    onMounted(async () => {
      await fetchStudents();
      await fetchClassNames();
    });

    const openEditStudentForm = (student) => {
      editingStudent.value = { ...student }
    }

    const cancelEdit = () => {
      editingStudent.value = null
    }

    const updateStudent = async () => {
      try {
        await axios.put(`http://localhost:5016/api/home/students/${editingStudent.value.id}`, editingStudent.value)
        cancelEdit()
        await fetchStudents()
      } catch (error) {
        console.error('Error updating student:', error)
      }
    }

    const showAttendanceDetails = (student) => {
      selectedStudent.value = student
    }

    const goBack = () => {
      selectedStudent.value = null
    }

    const filteredStudents = computed(() => {
      if (!searchTerm.value) {
        return students.value
      }
      const term = searchTerm.value.toLowerCase()
      return students.value.filter(student =>
        student.name.toLowerCase().includes(term)
      )
    })

    const countAttendanceStatus = (student, status) => {
      return student.attendanceRecords.filter(record => record.status === status).length
    }

    const calculateAbsenceRate = (student) => {
      const totalRecords = student.attendanceRecords.length
      const absentCount = countAttendanceStatus(student, 'absent')
      const cautionCount = countAttendanceStatus(student, 'warning')
      const effectiveAbsentCount = absentCount + Math.floor(cautionCount / 2)
      return ((effectiveAbsentCount / totalRecords) * 100).toFixed(0)
    }

    const fetchStudents = async () => {
      try {
        const response = await axios.get('http://localhost:5016/api/home/students')
        students.value = response.data
      } catch (error) {
        console.error('Error fetching students:', error)
      }
    }

    onMounted(fetchStudents)

    return {
      students,
      classNames,
      searchTerm,
      filteredStudents,
      countAttendanceStatus,
      calculateAbsenceRate,
      showAttendanceDetails,
      selectedStudent,
      goBack,
      editingStudent,
      openEditStudentForm,
      cancelEdit,
      updateStudent
    }
  }
}
</script>

<style scoped>
.detail {
  padding: 20px;
  justify-content: center; 
  align-items: center;   
  flex-direction: column; 
  min-height: 100vh;     
}

.search-container {
  margin-bottom: 20px;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th,
td {
  padding: 10px;
  text-align: center;
  vertical-align: middle;
  border-bottom: 1px solid #ddd;
}

th {
  background-color: #f2f2f2;
}

tbody {
  max-height: 400px;
  overflow-y: auto;
}
.attendance-details {
  margin-top: 20px;
}

.attendance-details table {
  width: 100%;
  border-collapse: collapse;
}

.attendance-details th,
.attendance-details td {
  padding: 10px;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

.back-button {
  background-color: #04AA6D;
  border: none;
  color: white;
  padding: 15px 32px;
  text-align: center;
  text-decoration: none;
  display: inline-block;
  font-size: 16px;
  margin: 4px 2px;
  cursor: pointer;
}

.back-button:hover {
  background-color: #45a049;  
}

.action-button {
  background-color: #04AA6D;
  border: none;
  color: white;
  padding: 8px 16px;
  text-align: center;
  text-decoration: none;
  display: inline-block;
  font-size: 14px;
  margin: 4px 2px;
  cursor: pointer;
}

.action-button:hover {
  background-color: #45a049;
}

.search-container {
  margin-bottom: 20px;
}

.search-container input[type="text"] {
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 16px;
  width: 300px;
}

.edit-form {
  margin-top: 20px;
}

.edit-form div {
  margin-bottom: 15px;
}

.edit-form label {
  display: block;
  font-weight: bold;
  margin-bottom: 5px;
}

.edit-form input[type="text"],
.edit-form input[type="date"],
.edit-form select {
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 14px;
  width: 100%;
}

.edit-form button[type="submit"] {
  background-color: #04AA6D;
  border: none;
  color: white;
  padding: 10px 20px;
  text-align: center;
  text-decoration: none;
  display: inline-block;
  font-size: 16px;
  margin-top: 10px;
  cursor: pointer;
  border-radius: 4px;
}

.edit-form button[type="submit"]:hover {
  background-color: #45a049;
}
</style>