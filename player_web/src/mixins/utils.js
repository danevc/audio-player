export default {
  data() {
    return {
      isMove: false
    }
  },
  methods: {
    normalizeTime(time) {
      if (time < 10)
        return `0${time}`;
      else
        return time;
    },
    secToMin(value) {
      var min = this.normalizeTime(Math.floor(value / 60));
      var sec = this.normalizeTime(value - min * 60);
      return `${min}:${sec}`;
    }
  }
}